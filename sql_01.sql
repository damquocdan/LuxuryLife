USE [TourBooking]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/7/2025 8:57:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Avatar] [nvarchar](255) NULL,
 CONSTRAINT [PK__Admin__719FE488CCBBC051] PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[BookingId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TourId] [int] NULL,
	[BookingDate] [datetime] NULL,
	[CheckInDate] [datetime] NULL,
	[CheckOutDate] [datetime] NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK__Booking__73951AED39332FF9] PRIMARY KEY CLUSTERED 
(
	[BookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Avatar] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[Dob] [date] NULL,
	[Demographics] [nvarchar](255) NULL,
	[Preferences] [nvarchar](255) NULL,
	[SearchHistory] [nvarchar](max) NULL,
	[Createdate] [datetime] NULL,
 CONSTRAINT [PK__Customer__A4AE64D8CED30739] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerInteraction]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInteraction](
	[InteractionId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TourId] [int] NULL,
	[InteractionType] [nvarchar](50) NULL,
	[InteractionDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[InteractionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favourite]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favourite](
	[FavoriteId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TourId] [int] NULL,
	[Sumprice] [decimal](18, 2) NULL,
 CONSTRAINT [PK__Favourit__CE74FAD5B2B8E927] PRIMARY KEY CLUSTERED 
(
	[FavoriteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Homestay]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Homestay](
	[HomestayId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Address] [nvarchar](255) NULL,
	[Image] [nvarchar](255) NULL,
	[PricePerNight] [decimal](18, 2) NULL,
	[ProviderId] [int] NULL,
	[TourId] [int] NULL,
 CONSTRAINT [PK__Homestay__EDCB5CBABC26500E] PRIMARY KEY CLUSTERED 
(
	[HomestayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listimagestour]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listimagestour](
	[ListimagestourId] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[ImageDescription] [nvarchar](255) NULL,
	[Createdate] [datetime] NULL,
 CONSTRAINT [PK__Listimag__C13099DB06DC99D8] PRIMARY KEY CLUSTERED 
(
	[ListimagestourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MomoPayment]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MomoPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[BookingId] [int] NULL,
	[CustomerId] [int] NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[PaymentDate] [datetime] NULL,
	[RequestId] [nvarchar](100) NULL,
	[PaymentUrl] [nvarchar](max) NULL,
	[TransactionId] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[NewId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Createdate] [datetime] NULL,
 CONSTRAINT [PK__News__7CC3777EEB9A1D67] PRIMARY KEY CLUSTERED 
(
	[NewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provider]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provider](
	[ProviderId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Avatar] [nvarchar](255) NULL,
	[Phone] [nvarchar](15) NULL,
	[Address] [nvarchar](255) NULL,
	[Rating] [decimal](3, 2) NULL,
	[Createdate] [datetime] NULL,
 CONSTRAINT [PK__Provider__B54C687D284498A8] PRIMARY KEY CLUSTERED 
(
	[ProviderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TourId] [int] NULL,
	[Rating] [decimal](3, 2) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[Createdate] [datetime] NULL,
 CONSTRAINT [PK__Review__74BC79CE402D27CA] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NULL,
	[TourId] [int] NULL,
 CONSTRAINT [PK__Service__C51BB00AB64FBE5D] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tour]    Script Date: 3/7/2025 8:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tour](
	[TourId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Image] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ServiceId] [int] NULL,
	[HomestayId] [int] NULL,
	[PricePerson] [decimal](18, 2) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Price] [decimal](18, 2) NULL,
	[Status] [nvarchar](50) NULL,
	[Createdate] [datetime] NULL,
	[ProviderId] [int] NULL,
 CONSTRAINT [PK__Tour__604CEA3002475256] PRIMARY KEY CLUSTERED 
(
	[TourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT (getdate()) FOR [BookingDate]
GO
ALTER TABLE [dbo].[Contact] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Contact] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[CustomerInteraction] ADD  DEFAULT (getdate()) FOR [InteractionDate]
GO
ALTER TABLE [dbo].[Listimagestour] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[MomoPayment] ADD  DEFAULT ('Pending') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[MomoPayment] ADD  DEFAULT (getdate()) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[Provider] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[Review] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[Tour] ADD  DEFAULT (getdate()) FOR [Createdate]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__Custome__5812160E] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__Custome__5812160E]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__TourId__59063A47] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__TourId__59063A47]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[CustomerInteraction]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[CustomerInteraction]  WITH CHECK ADD FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Favourite]  WITH CHECK ADD  CONSTRAINT [FK__Favourite__Custo__4E88ABD4] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Favourite] CHECK CONSTRAINT [FK__Favourite__Custo__4E88ABD4]
GO
ALTER TABLE [dbo].[Favourite]  WITH CHECK ADD  CONSTRAINT [FK__Favourite__TourI__4F7CD00D] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Favourite] CHECK CONSTRAINT [FK__Favourite__TourI__4F7CD00D]
GO
ALTER TABLE [dbo].[Homestay]  WITH CHECK ADD  CONSTRAINT [FK_Homestay_Tour] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Homestay] CHECK CONSTRAINT [FK_Homestay_Tour]
GO
ALTER TABLE [dbo].[Homestay]  WITH CHECK ADD  CONSTRAINT [FK_Homestay_Tour_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Homestay] CHECK CONSTRAINT [FK_Homestay_Tour_TourId]
GO
ALTER TABLE [dbo].[Listimagestour]  WITH CHECK ADD  CONSTRAINT [FK__Listimage__TourI__46E78A0C] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Listimagestour] CHECK CONSTRAINT [FK__Listimage__TourI__46E78A0C]
GO
ALTER TABLE [dbo].[MomoPayment]  WITH CHECK ADD FOREIGN KEY([BookingId])
REFERENCES [dbo].[Booking] ([BookingId])
GO
ALTER TABLE [dbo].[MomoPayment]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK__Review__Customer__534D60F1] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK__Review__Customer__534D60F1]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK__Review__TourId__5441852A] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK__Review__TourId__5441852A]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__TourId__4BAC3F29] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([TourId])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__TourId__4BAC3F29]
GO
ALTER TABLE [dbo].[Tour]  WITH CHECK ADD  CONSTRAINT [FK__Tour__ProviderId__4316F928] FOREIGN KEY([ProviderId])
REFERENCES [dbo].[Provider] ([ProviderId])
GO
ALTER TABLE [dbo].[Tour] CHECK CONSTRAINT [FK__Tour__ProviderId__4316F928]
GO
