DECLARE @CustomerId INT = 1; -- Thay [CustomerId] bằng ID của khách hàng cụ thể
DECLARE @Query NVARCHAR(500) = '[Query]'; -- Thay [Query] bằng từ khóa tìm kiếm (nếu có)
DECLARE @Description NVARCHAR(500) = '[Description]'; -- Thay [Description] bằng mô tả tìm kiếm (nếu có)

-- **1. Lấy danh sách tour yêu thích (Priority 1)**
SELECT 
    t.TourId,
    t.Name,
    t.Description,
    t.Status,
    t.Createdate,
    p.ProviderId,
    p.Name,
    1 AS Priority,
    100 AS MatchScore
FROM Tour t
INNER JOIN Provider p ON t.ProviderId = p.ProviderId
INNER JOIN Favourite f ON t.TourId = f.TourId
WHERE t.Status = 'Active'
    AND f.CustomerId = @CustomerId
    AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
    AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%');

-- **2. Gợi ý dựa trên sở thích, lịch sử tìm kiếm (Priority 2)**
WITH CustomerKeywords AS (
    SELECT 
        value AS Keyword 
    FROM Customer c
    CROSS APPLY STRING_SPLIT(c.Preferences + ' ' + c.SearchHistory + ' ' + c.Demographics, ' ')
    WHERE c.CustomerId = @CustomerId
        AND value <> ''
)
SELECT 
    t.TourId,
    t.Name,
    t.Description,
    t.Status,
    t.Createdate,
    p.ProviderId,
    p.Name,
    2 AS Priority,
    COUNT(ck.Keyword) AS MatchScore
FROM Tour t
INNER JOIN Provider p ON t.ProviderId = p.ProviderId
CROSS JOIN CustomerKeywords ck
WHERE t.Status = 'Active'
    AND t.TourId NOT IN (
        SELECT TourId 
        FROM Favourite 
        WHERE CustomerId = @CustomerId
    )
    AND LOWER(t.Description) LIKE '%' + LOWER(ck.Keyword) + '%'
    AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
    AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%')
GROUP BY 
    t.TourId,
    t.Name,
    t.Description,
    t.Status,
    t.Createdate,
    p.ProviderId,
    p.Name
HAVING COUNT(ck.Keyword) > 0
ORDER BY MatchScore DESC;

-- **3. Gợi ý dựa trên tour đã đặt và đánh giá (Priority 3)**
WITH BookedTours AS (
    SELECT DISTINCT TourId
    FROM Booking
    WHERE CustomerId = @CustomerId
),
ReviewedTours AS (
    SELECT DISTINCT TourId
    FROM Review
    WHERE CustomerId = @CustomerId
),
SimilarCustomers AS (
    SELECT DISTINCT r.CustomerId
    FROM Review r
    WHERE r.TourId IN (SELECT TourId FROM BookedTours)
        OR r.TourId IN (SELECT TourId FROM ReviewedTours)
),
SimilarReviews AS (
    SELECT DISTINCT r.TourId
    FROM Review r
    WHERE r.CustomerId IN (SELECT CustomerId FROM SimilarCustomers)
        AND r.Rating >= 4
),
BookedServices AS (
    SELECT DISTINCT s.ServiceId
    FROM Service s
    INNER JOIN BookedTours bt ON s.TourId = bt.TourId
),
BookedHomestays AS (
    SELECT DISTINCT h.HomestayId
    FROM Homestay h
    INNER JOIN BookedTours bt ON h.TourId = bt.TourId
),
ContentBasedTours AS (
    SELECT DISTINCT t.TourId
    FROM Tour t
    INNER JOIN Service s ON t.TourId = s.TourId
    WHERE s.ServiceId IN (SELECT ServiceId FROM BookedServices)
    UNION
    SELECT DISTINCT t.TourId
    FROM Tour t
    INNER JOIN Homestay h ON t.TourId = h.TourId
    WHERE h.HomestayId IN (SELECT HomestayId FROM BookedHomestays)
),
RecommendedTourIds AS (
    SELECT TourId FROM SimilarReviews
    UNION
    SELECT TourId FROM ContentBasedTours
)
SELECT 
    t.TourId,
    t.Name,
    t.Description,
    t.Status,
    t.Createdate,
    p.ProviderId,
    p.Name,
    3 AS Priority,
    50 AS MatchScore
FROM Tour t
INNER JOIN Provider p ON t.ProviderId = p.ProviderId
INNER JOIN RecommendedTourIds rt ON t.TourId = rt.TourId
WHERE t.Status = 'Active'
    AND t.TourId NOT IN (
        SELECT TourId 
        FROM Favourite 
        WHERE CustomerId = @CustomerId
    )
    AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
    AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%');

-- **4. Các tour còn lại (Priority 4)**
SELECT 
    t.TourId,
    t.Name,
    t.Description,
    t.Status,
    t.Createdate,
    p.ProviderId,
    p.Name,
    4 AS Priority,
    0 AS MatchScore
FROM Tour t
INNER JOIN Provider p ON t.ProviderId = p.ProviderId
WHERE t.Status = 'Active'
    AND t.TourId NOT IN (
        SELECT TourId 
        FROM Favourite 
        WHERE CustomerId = @CustomerId
    )
    AND t.TourId NOT IN (
        SELECT t2.TourId
        FROM Tour t2
        CROSS JOIN (
            SELECT 
                value AS Keyword
            FROM Customer c
            CROSS APPLY STRING_SPLIT(c.Preferences + ' ' + c.SearchHistory + ' ' + c.Demographics, ' ')
            WHERE c.CustomerId = @CustomerId
                AND value <> ''
        ) ck
        WHERE LOWER(t2.Description) LIKE '%' + LOWER(ck.Keyword) + '%'
        GROUP BY t2.TourId
        HAVING COUNT(ck.Keyword) > 0
    )
    AND t.TourId NOT IN (
        SELECT TourId 
        FROM (
            SELECT DISTINCT r.TourId
            FROM Review r
            INNER JOIN (
                SELECT DISTINCT r2.CustomerId
                FROM Review r2
                WHERE r2.TourId IN (SELECT TourId FROM Booking WHERE CustomerId = @CustomerId)
                    OR r2.TourId IN (SELECT TourId FROM Review WHERE CustomerId = @CustomerId)
            ) sc ON r.CustomerId = sc.CustomerId
            WHERE r.Rating >= 4
            UNION
            SELECT DISTINCT t3.TourId
            FROM Tour t3
            INNER JOIN Service s ON t3.TourId = s.TourId
            WHERE s.ServiceId IN (
                SELECT DISTINCT s2.ServiceId
                FROM Service s2
                INNER JOIN Booking b ON s2.TourId = b.TourId
                WHERE b.CustomerId = @CustomerId
            )
            UNION
            SELECT DISTINCT t4.TourId
            FROM Tour t4
            INNER JOIN Homestay h ON t4.TourId = h.TourId
            WHERE h.HomestayId IN (
                SELECT DISTINCT h2.HomestayId
                FROM Homestay h2
                INNER JOIN Booking b ON h2.TourId = b.TourId
                WHERE b.CustomerId = @CustomerId
            )
        ) rt
    )
    AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
    AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%');

-- **5. Kết hợp tất cả danh sách tour theo thứ tự ưu tiên**
-- **5. Kết hợp tất cả danh sách tour theo thứ tự ưu tiên**
WITH AllTours AS (
    -- Priority 1: Tour yêu thích
    SELECT 
        t.TourId,
        t.Name AS TourName,
        t.Description,
        t.Status,
        t.Createdate,
        p.ProviderId,
        p.Name AS ProviderName,
        1 AS Priority,
        100 AS MatchScore
    FROM Tour t
    INNER JOIN Provider p ON t.ProviderId = p.ProviderId
    INNER JOIN Favourite f ON t.TourId = f.TourId
    WHERE t.Status = 'Active'
        AND f.CustomerId = @CustomerId
        AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
        AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%')

    UNION ALL

    -- Priority 2: Gợi ý dựa trên sở thích, lịch sử tìm kiếm
    SELECT 
        t.TourId,
        t.Name AS TourName,
        t.Description,
        t.Status,
        t.Createdate,
        p.ProviderId,
        p.Name AS ProviderName,
        2 AS Priority,
        COUNT(ck.Keyword) AS MatchScore
    FROM Tour t
    INNER JOIN Provider p ON t.ProviderId = p.ProviderId
    CROSS JOIN (
        SELECT 
            value AS Keyword
        FROM Customer c
        CROSS APPLY STRING_SPLIT(c.Preferences + ' ' + c.SearchHistory + ' ' + c.Demographics, ' ')
        WHERE c.CustomerId = @CustomerId
            AND value <> ''
    ) ck
    WHERE t.Status = 'Active'
        AND t.TourId NOT IN (
            SELECT TourId 
            FROM Favourite 
            WHERE CustomerId = @CustomerId
        )
        AND LOWER(t.Description) LIKE '%' + LOWER(ck.Keyword) + '%'
        AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
        AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%')
    GROUP BY 
        t.TourId,
        t.Name,
        t.Description,
        t.Status,
        t.Createdate,
        p.ProviderId,
        p.Name
    HAVING COUNT(ck.Keyword) > 0

    UNION ALL

    -- Priority 3: Gợi ý dựa trên tour đã đặt và đánh giá
    SELECT 
        t.TourId,
        t.Name AS TourName,
        t.Description,
        t.Status,
        t.Createdate,
        p.ProviderId,
        p.Name AS ProviderName,
        3 AS Priority,
        50 AS MatchScore
    FROM Tour t
    INNER JOIN Provider p ON t.ProviderId = p.ProviderId
    INNER JOIN (
        SELECT DISTINCT r.TourId
        FROM Review r
        INNER JOIN (
            SELECT DISTINCT r2.CustomerId
            FROM Review r2
            WHERE r2.TourId IN (SELECT TourId FROM Booking WHERE CustomerId = @CustomerId)
                OR r2.TourId IN (SELECT TourId FROM Review WHERE CustomerId = @CustomerId)
        ) sc ON r.CustomerId = sc.CustomerId
        WHERE r.Rating >= 4
        UNION
        SELECT DISTINCT t3.TourId
        FROM Tour t3
        INNER JOIN Service s ON t3.TourId = s.TourId
        WHERE s.ServiceId IN (
            SELECT DISTINCT s2.ServiceId
            FROM Service s2
            INNER JOIN Booking b ON s2.TourId = b.TourId
            WHERE b.CustomerId = @CustomerId
        )
        UNION
        SELECT DISTINCT t4.TourId
        FROM Tour t4
        INNER JOIN Homestay h ON t4.TourId = h.TourId
        WHERE h.HomestayId IN (
            SELECT DISTINCT h2.HomestayId
            FROM Homestay h2
            INNER JOIN Booking b ON h2.TourId = b.TourId
            WHERE b.CustomerId = @CustomerId
        )
    ) rt ON t.TourId = rt.TourId
    WHERE t.Status = 'Active'
        AND t.TourId NOT IN (
            SELECT TourId 
            FROM Favourite 
            WHERE CustomerId = @CustomerId
        )
        AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
        AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%')

    UNION ALL

    -- Priority 4: Các tour còn lại
    SELECT 
        t.TourId,
        t.Name AS TourName,
        t.Description,
        t.Status,
        t.Createdate,
        p.ProviderId,
        p.Name AS ProviderName,
        4 AS Priority,
        0 AS MatchScore
    FROM Tour t
    INNER JOIN Provider p ON t.ProviderId = p.ProviderId
    WHERE t.Status = 'Active'
        AND t.TourId NOT IN (
            SELECT TourId 
            FROM Favourite
            WHERE CustomerId = @CustomerId
        )
        AND t.TourId NOT IN (
            SELECT t2.TourId
            FROM Tour t2
            CROSS JOIN (
                SELECT 
                    value AS Keyword
                FROM Customer c
                CROSS APPLY STRING_SPLIT(c.Preferences + ' ' + c.SearchHistory + ' ' + c.Demographics, ' ')
                WHERE c.CustomerId = @CustomerId
                    AND value <> ''
            ) ck
            WHERE LOWER(t2.Description) LIKE '%' + LOWER(ck.Keyword) + '%'
            GROUP BY t2.TourId
            HAVING COUNT(ck.Keyword) > 0
        )
        AND t.TourId NOT IN (
            SELECT TourId 
            FROM (
                SELECT DISTINCT r.TourId
                FROM Review r
                INNER JOIN (
                    SELECT DISTINCT r2.CustomerId
                    FROM Review r2
                    WHERE r2.TourId IN (SELECT TourId FROM Booking WHERE CustomerId = @CustomerId)
                        OR r2.TourId IN (SELECT TourId FROM Review WHERE CustomerId = @CustomerId)
                ) sc ON r.CustomerId = sc.CustomerId
                WHERE r.Rating >= 4
                UNION
                SELECT DISTINCT t3.TourId
                FROM Tour t3
                INNER JOIN Service s ON t3.TourId = s.TourId
                WHERE s.ServiceId IN (
                    SELECT DISTINCT s2.ServiceId
                    FROM Service s2
                    INNER JOIN Booking b ON s2.TourId = b.TourId
                    WHERE b.CustomerId = @CustomerId
                )
                UNION
                SELECT DISTINCT t4.TourId
                FROM Tour t4
                INNER JOIN Homestay h ON t4.TourId = h.TourId
                WHERE h.HomestayId IN (
                    SELECT DISTINCT h2.HomestayId
                    FROM Homestay h2
                    INNER JOIN Booking b ON h2.TourId = b.TourId
                    WHERE b.CustomerId = @CustomerId
                )
            ) rt
        )
        AND (@Query IS NULL OR LOWER(t.Name) LIKE '%' + LOWER(@Query) + '%' OR LOWER(t.Description) LIKE '%' + LOWER(@Query) + '%')
        AND (@Description IS NULL OR LOWER(t.Description) LIKE '%' + LOWER(@Description) + '%')
)
SELECT 
    TourId,
    TourName,
    Description,
    Status,
    Createdate,
    ProviderId,
    ProviderName,
    Priority,
    MatchScore
FROM AllTours
ORDER BY Priority, MatchScore DESC, Createdate DESC;