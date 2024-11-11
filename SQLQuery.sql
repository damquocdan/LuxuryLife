CREATE TABLE [dbo].[Admin](
	[adminId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[actionHistory] [text] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[adminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[bookingId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[bookingDate] [date] NOT NULL,
	[checkInDate] [date] NULL,
	[checkOutDate] [date] NULL,
	[status] [varchar](50) NULL,
	[totalPrice] [float] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[bookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingService]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingService](
	[bookingServiceId] [int] IDENTITY(1,1) NOT NULL,
	[bookingId] [int] NULL,
	[serviceName] [varchar](255) NULL,
	[servicePrice] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[bookingServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[customerId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[phone] [varchar](15) NULL,
	[address] [varchar](255) NULL,
	[dob] [date] NULL,
	[demographics] [varchar](255) NULL,
	[preferences] [text] NULL,
	[searchHistory] [text] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerSupport]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerSupport](
	[supportId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[issueDescription] [varchar](500) NULL,
	[supportResponse] [varchar](500) NULL,
	[status] [varchar](50) NULL,
	[createDate] [datetime] NULL,
	[resolvedDate] [datetime] NULL,
	[isResolved] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[supportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favorite]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favorite](
	[favoriteId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NOT NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[favoriteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Homestay]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Homestay](
	[homestayId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[description] [text] NULL,
	[location] [varchar](255) NOT NULL,
	[pricePerNight] [float] NULL,
	[availability] [bit] NULL,
	[rating] [float] NULL,
	[providerId] [int] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[homestayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[imageId] [int] IDENTITY(1,1) NOT NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[imageUrl] [varchar](255) NOT NULL,
	[imageDescription] [varchar](255) NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[imageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[newsId] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](255) NOT NULL,
	[content] [text] NOT NULL,
	[publicationDate] [date] NOT NULL,
	[authorId] [int] NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[providerId] [int] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[newsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notificationId] [int] IDENTITY(1,1) NOT NULL,
	[recipientId] [int] NULL,
	[recipientType] [varchar](50) NULL,
	[message] [varchar](500) NULL,
	[notificationDate] [datetime] NULL,
	[isRead] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[notificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[paymentId] [int] IDENTITY(1,1) NOT NULL,
	[bookingId] [int] NULL,
	[amount] [float] NOT NULL,
	[paymentMethod] [varchar](50) NULL,
	[paymentStatus] [varchar](50) NULL,
	[paymentDate] [date] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[paymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[methodId] [int] IDENTITY(1,1) NOT NULL,
	[methodName] [varchar](50) NULL,
	[description] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[methodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provider]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provider](
	[providerId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[phone] [varchar](15) NULL,
	[address] [varchar](255) NULL,
	[revenue] [float] NULL,
	[rating] [float] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[providerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rating]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[ratingId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[ratingValue] [int] NULL,
	[reviewComment] [varchar](500) NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ratingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[reviewId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[rating] [int] NULL,
	[comment] [text] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[reviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[serviceId] [int] IDENTITY(1,1) NOT NULL,
	[serviceName] [varchar](255) NOT NULL,
	[description] [text] NULL,
	[price] [float] NULL,
	[homestayId] [int] NULL,
	[tourId] [int] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[serviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tour]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tour](
	[tourId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[description] [text] NULL,
	[destination] [varchar](255) NOT NULL,
	[startDate] [date] NULL,
	[endDate] [date] NULL,
	[pricePerPerson] [float] NULL,
	[maxParticipants] [int] NULL,
	[availableSeats] [int] NULL,
	[rating] [float] NULL,
	[providerId] [int] NULL,
	[createDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[tourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourPackage]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourPackage](
	[packageId] [int] IDENTITY(1,1) NOT NULL,
	[tourId] [int] NULL,
	[packageName] [varchar](255) NULL,
	[packagePrice] [decimal](18, 2) NULL,
	[description] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[packageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourSchedule]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourSchedule](
	[scheduleId] [int] IDENTITY(1,1) NOT NULL,
	[tourId] [int] NULL,
	[day] [int] NULL,
	[activityDescription] [varchar](500) NULL,
	[location] [varchar](255) NULL,
	[scheduleDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[scheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[transactionId] [int] IDENTITY(1,1) NOT NULL,
	[paymentId] [int] NULL,
	[customerId] [int] NULL,
	[amount] [decimal](18, 2) NULL,
	[transactionDate] [datetime] NULL,
	[transactionStatus] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[transactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 11/6/2024 9:45:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher](
	[voucherId] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NULL,
	[discountPercentage] [decimal](5, 2) NULL,
	[validFrom] [datetime] NULL,
	[validUntil] [datetime] NULL,
	[minSpendAmount] [decimal](18, 2) NULL,
	[status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[voucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[CustomerSupport] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[CustomerSupport] ADD  DEFAULT ((0)) FOR [isResolved]
GO
ALTER TABLE [dbo].[Favorite] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Homestay] ADD  DEFAULT ((1)) FOR [availability]
GO
ALTER TABLE [dbo].[Homestay] ADD  DEFAULT ((0)) FOR [rating]
GO
ALTER TABLE [dbo].[Homestay] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Image] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [notificationDate]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT ((0)) FOR [isRead]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Provider] ADD  DEFAULT ((0)) FOR [revenue]
GO
ALTER TABLE [dbo].[Provider] ADD  DEFAULT ((0)) FOR [rating]
GO
ALTER TABLE [dbo].[Provider] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Rating] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Review] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Service] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Tour] ADD  DEFAULT ((0)) FOR [rating]
GO
ALTER TABLE [dbo].[Tour] ADD  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [transactionDate]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[BookingService]  WITH CHECK ADD FOREIGN KEY([bookingId])
REFERENCES [dbo].[Booking] ([bookingId])
GO
ALTER TABLE [dbo].[CustomerSupport]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Favorite]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Favorite]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
GO
ALTER TABLE [dbo].[Favorite]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
GO
ALTER TABLE [dbo].[Homestay]  WITH CHECK ADD FOREIGN KEY([providerId])
REFERENCES [dbo].[Provider] ([providerId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD FOREIGN KEY([authorId])
REFERENCES [dbo].[Admin] ([adminId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD FOREIGN KEY([providerId])
REFERENCES [dbo].[Provider] ([providerId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([bookingId])
REFERENCES [dbo].[Booking] ([bookingId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD FOREIGN KEY([homestayId])
REFERENCES [dbo].[Homestay] ([homestayId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Tour]  WITH CHECK ADD FOREIGN KEY([providerId])
REFERENCES [dbo].[Provider] ([providerId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[TourPackage]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
GO
ALTER TABLE [dbo].[TourSchedule]  WITH CHECK ADD FOREIGN KEY([tourId])
REFERENCES [dbo].[Tour] ([tourId])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([paymentId])
REFERENCES [dbo].[Payment] ([paymentId])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD CHECK  (([rating]>=(1) AND [rating]<=(5)))
GO
INSERT INTO [LuxuryLife].[dbo].[Admin] ([name], [email], [password], [actionHistory], [createDate])
VALUES ('Admin Name', 'admin@gmail.com', HASHBYTES('SHA2_256', '12345678'), 'Initial action', GETDATE());
INSERT INTO [LuxuryLife].[dbo].[Admin] ([name], [email], [password], [actionHistory], [createDate])
VALUES ('Admin Dan', 'admindan@gmail.com', '12345678', 'Initial action', GETDATE());
