# Library Management System

## 1. Models

### Member

-**Id * *(GUID)
- **UserName * *(string)
-**Email * *(string)
-**Password * *(string, hashed)
- **Role * *(enum: Admin, Librarian, Member)
- **DateJoined** (DateTime)
-**ProfilePicture * *(string)
-**IsBanned * *(bool)
-**IsLocked * *(bool)
 -**CreatedAt * *(DateTime)
 -**UpdatedAt * *(DateTime)

### Book

-**Id * *(GUID)
- **Title * *(string)
-**Author * *(string)
-**ISBN * *(string)
-**Publisher * *(string)
-**PublishedDate * *(DateTime)
- **Category * *(string)
-**Description * *(string)
-**CopiesAvailable * *(int)
-**TotalCopies * *(int)
-**IsAvailable * *(bool)

### Category

- **Id** (GUID)
- **Name** (string)
- **Description** (string)

### Loan

- **Id** (GUID)
- **BookId** (GUID)
- **UserId** (GUID)
- **MemberId** (GUID)
- **LoanDate** (DateTime)
- **BorrowDate** (DateTime)
- **DueDate** (DateTime)
- **ReturnDate** (DateTime, nullable)
- **Status** (enum: Loaned, Returned, Overdue)


## 2. API Endpoints

### User Management

- **POST /api/users/register**
- **POST /api/users/login**
- **GET /api/users/{id}**
- **PUT /api/users/{id}**
- **DELETE /api/users/{id}**

### Book Management

- **POST /api/books**
- **GET /api/books**
- **GET /api/books/{id}**
- **PUT /api/books/{id}**
- **DELETE /api/books/{id}**

### Loan Management

- **POST /api/loans**
- **GET /api/loans**
- **GET /api/loans/{id}**
- **PUT /api/loans/{id}**
- **DELETE /api/loans/{id}**

## 3. Additional Features

### Reservation Management
### Fine Management
### Review Management
### Reports
