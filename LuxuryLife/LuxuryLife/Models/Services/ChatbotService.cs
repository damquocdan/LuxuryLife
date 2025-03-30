using LuxuryLife.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class ChatbotService
{
    private readonly TourBookingContext _context;
    private readonly Random _random = new Random();

    public ChatbotService(TourBookingContext context)
    {
        _context = context;
    }

    public string ProcessMessage(string message, int customerId)
    {
        message = message.ToLower().Trim();
        var customer = _context.Customers.Find(customerId);
        string response;

        if (message.Contains("tìm tour") || message.Contains("tour nào") || message.Contains("gợi ý tour") || ContainsKeyword(message))
        {
            response = SearchTours(message, customer);
        }
        else if (message.Contains("giá") || message.Contains("bao nhiêu tiền") || message.Contains("chi phí"))
        {
            response = GetPriceInfo(message);
        }
        else if (message.Contains("ngày") || message.Contains("khi nào") || message.Contains("lịch trình") || IsDateQuery(message))
        {
            response = GetDateInfo(message);
        }
        else if (message.Contains("đặt tour") || message.Contains("làm sao đặt") || message.Contains("book tour"))
        {
            response = "Bạn hãy cho tôi biết tên tour và ngày khởi hành nhé! Thanh toán bằng chuyển khoản, thẻ tín dụng hay tiền mặt? Tôi sẽ giúp bạn từng bước.";
        }
        else if (message.Contains("thanh toán") || message.Contains("cách trả tiền"))
        {
            response = "Bạn có thể thanh toán qua chuyển khoản (Vietcombank: 123456789), thẻ tín dụng, hoặc tiền mặt tại văn phòng. Cần tôi gửi chi tiết không?";
        }
        else if (message.Contains("homestay") || message.Contains("nơi ở"))
        {
            response = GetHomestayInfo();
        }
        else if (message.Contains("dịch vụ") || message.Contains("bao gồm gì"))
        {
            response = GetServiceInfo();
        }
        else if (message.Contains("hủy tour") || message.Contains("hủy đặt"))
        {
            response = "Để hủy tour, bạn liên hệ qua hotline +84 368 966 992 hoặc email support@travelinvietnam.com với mã đặt tour. Bạn có mã chưa?";
        }
        else if (message.Contains("xin chào") || message.Contains("hi") || message.Contains("hello"))
        {
            response = $"Chào {customer?.Name ?? "bạn"}! Bạn muốn tìm tour biển, tour núi, hay hỏi giá hôm nay?";
        }
        else if (message.Contains("cảm ơn") || message.Contains("thanks"))
        {
            response = GetRandomThankYouResponse(customer?.Name);
        }
        else if (message.Contains("tạm biệt") || message.Contains("bye"))
        {
            response = "Tạm biệt! Chúc bạn có chuyến đi vui vẻ. Gọi tôi nếu cần nhé!";
        }
        else
        {
            response = "Tôi chưa hiểu lắm. Bạn muốn tìm tour (ví dụ: 'tour Đà Nẵng', 'tour rẻ'), giá, hay cách đặt tour? Nói thêm để tôi giúp nhé!";
        }

        response += "\n" + GetRandomSuggestion();

        var chatHistory = new ChatHistory
        {
            CustomerId = customerId,
            Message = message,
            Response = response,
            CreatedDate = DateTime.Now
        };
        _context.ChatHistories.Add(chatHistory);
        _context.SaveChanges();

        return response;
    }

    private bool ContainsKeyword(string message)
    {
        string[] keywords = { "biển", "núi", "rẻ", "ngắn ngày", "dài ngày", "đà nẵng", "phú quốc", "hội an", "sapa", "nha trang", "đà lạt", "huế", "hà giang", "quy nhơn", "phú yên" };
        return keywords.Any(k => message.Contains(k));
    }

    private bool IsDateQuery(string message)
    {
        return Regex.IsMatch(message, @"\d{1,2}/\d{1,2}") || message.Contains("tháng");
    }

    private string SearchTours(string message, Customer customer)
    {
        var tours = _context.Tours
            .Where(t => t.Status == "Active")
            .AsQueryable();

        // Lọc theo sở thích khách hàng
        if (!string.IsNullOrEmpty(customer?.Preferences))
        {
            var preferences = customer.Preferences.Split(',');
            tours = tours.Where(t => preferences.Any(p => t.Description.Contains(p) || t.Name.Contains(p)));
        }

        // Lọc theo từ khóa cụ thể
        if (message.Contains("biển"))
            tours = tours.Where(t => t.Name.Contains("biển") || t.Description.Contains("biển") || t.Name.Contains("nha trang") || t.Name.Contains("phú quốc") || t.Name.Contains("quy nhơn"));
        if (message.Contains("núi"))
            tours = tours.Where(t => t.Name.Contains("núi") || t.Description.Contains("núi") || t.Name.Contains("sapa") || t.Name.Contains("hà giang") || t.Name.Contains("đà lạt"));
        if (message.Contains("rẻ"))
            tours = tours.Where(t => t.PricePerson <= 1500000).OrderBy(t => t.Price);
        if (message.Contains("ngắn ngày"))
            tours = tours.Where(t => t.Day <= 3);
        if (message.Contains("dài ngày"))
            tours = tours.Where(t => t.Day > 3);

        // Lọc theo địa điểm cụ thể
        string[] locations = { "đà nẵng", "phú quốc", "hội an", "sapa", "nha trang", "đà lạt", "huế", "hà giang", "quy nhơn", "phú yên" };
        foreach (var loc in locations)
        {
            if (message.Contains(loc))
                tours = tours.Where(t => t.Name.ToLower().Contains(loc));
        }

        var result = tours.Take(3)
            .Select(t => $"{t.Name} - {t.Day} ngày - Giá: {t.PricePerson:N0} VNĐ/người - Khởi hành: {(t.StartDate.HasValue ? t.StartDate.Value.ToString("dd/MM/yyyy") : "Chưa xác định")}")
            .ToList();

        return result.Any()
            ? $"Danh sách tour phù hợp:\n{string.Join("\n", result)}\nBạn thích tour nào? Muốn đặt hay xem thêm chi tiết?"
            : "Không tìm thấy tour phù hợp. Thử nói 'tour biển', 'tour rẻ', hoặc địa điểm cụ thể như 'tour Đà Nẵng' nhé!";
    }

    private string GetPriceInfo(string message)
    {
        var tours = _context.Tours
            .Where(t => t.Status == "Active")
            .AsQueryable();

        if (message.Contains("rẻ"))
            tours = tours.OrderBy(t => t.PricePerson);
        else if (Regex.IsMatch(message, @"\d+"))
        {
            var match = Regex.Match(message, @"\d+").Value;
            if (int.TryParse(match, out int price))
                tours = tours.Where(t => t.PricePerson <= price * 1000000); // Giả sử giá nhập vào là triệu VNĐ
        }

        var result = tours.Take(3)
            .Select(t => $"{t.Name} - Giá: {t.PricePerson:N0} VNĐ/người - {t.Day} ngày")
            .ToList();

        return result.Any()
            ? $"Tour giá tốt:\n{string.Join("\n", result)}\nBạn muốn tour trong khoảng giá bao nhiêu?"
            : "Không có tour nào phù hợp. Bạn muốn tôi tìm giá khác không?";
    }

    private string GetDateInfo(string message)
    {
        var tours = _context.Tours
            .Where(t => t.Status == "Active" && t.StartDate.HasValue && t.EndDate.HasValue && t.StartDate.Value > DateTime.Now)
            .AsQueryable();

        if (IsDateQuery(message))
        {
            var match = Regex.Match(message, @"\d{1,2}/\d{1,2}");
            if (DateTime.TryParseExact(match.Value, "d/M", null, System.Globalization.DateTimeStyles.None, out DateTime queryDate))
            {
                queryDate = queryDate.AddYears(DateTime.Now.Year - queryDate.Year);
                tours = tours.Where(t => t.StartDate.Value.Month == queryDate.Month && t.StartDate.Value.Day == queryDate.Day);
            }
        }

        var result = tours.OrderBy(t => t.StartDate)
            .Take(3)
            .Select(t => $"{t.Name} - Khởi hành: {t.StartDate.Value.ToString("dd/MM/yyyy")} - {t.Day} ngày")
            .ToList();

        return result.Any()
            ? $"Tour theo ngày:\n{string.Join("\n", result)}\nBạn muốn đặt tour nào?"
            : "Không có tour cho ngày đó. Bạn muốn tôi tìm ngày gần nhất không?";
    }

    private string GetHomestayInfo()
    {
        var homestays = _context.Tours
            .Where(t => t.Status == "Active" && t.HomestayId.HasValue)
            .Take(3)
            .Select(t => $"{t.Name} (Có homestay) - Giá: {t.PricePerson:N0} VNĐ/người")
            .ToList();

        return homestays.Any()
            ? $"Tour có homestay:\n{string.Join("\n", homestays)}\nBạn muốn biết thêm về tour nào?"
            : "Chưa có tour nào có homestay. Thử hỏi tour khác nhé!";
    }

    private string GetServiceInfo()
    {
        var services = _context.Tours
            .Where(t => t.Status == "Active" && t.ServiceId.HasValue)
            .Take(3)
            .Select(t => $"{t.Name} (Ăn uống, xe đưa đón, hướng dẫn viên) - Giá: {t.PricePerson:N0} VNĐ/người")
            .ToList();

        return services.Any()
            ? $"Tour có dịch vụ:\n{string.Join("\n", services)}\nBạn muốn biết chi tiết tour nào?"
            : "Chưa có tour nào có dịch vụ đặc biệt. Hỏi thêm nhé!";
    }

    private string GetRandomThankYouResponse(string customerName)
    {
        string[] responses = {
            $"Không có gì {customerName ?? "bạn"}! Bạn còn cần giúp gì không?",
            $"Cảm ơn bạn! Muốn tôi tìm tour mới không?",
            $"Vui được giúp {customerName ?? "bạn"}! Bạn sắp đi du lịch chưa?"
        };
        return responses[_random.Next(responses.Length)];
    }

    private string GetRandomSuggestion()
    {
        string[] suggestions = {
            "Bạn muốn tour biển, núi hay giá rẻ?",
            "Bạn cần giúp đặt tour hay xem giá không?",
            "Bạn muốn đi vào ngày nào? Ví dụ: 2/9?",
            "Bạn thích tour ngắn ngày hay dài ngày hơn?"
        };
        return suggestions[_random.Next(suggestions.Length)];
    }
}