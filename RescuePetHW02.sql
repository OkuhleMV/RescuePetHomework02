CREATE DATABASE RescuePetHW02

USE RescuePetHW02

CREATE TABLE [Status] (
    StatusID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL
);

CREATE TABLE [Location] (
    LocationID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL
);

CREATE TABLE [Type] (
    TypeID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL
);

CREATE TABLE Breed (
    BreedID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL,
	TypeID INT,
	FOREIGN KEY (TypeID) REFERENCES [Type](TypeID)
);

CREATE TABLE [User] ( 
    UserID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL
);

CREATE TABLE Gender (
    GenderID INT identity(1,1) PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL
);

CREATE TABLE Donation (
    DonationID INT identity(1,1) PRIMARY KEY,
    UserID INT,
    Amount DECIMAL(10,2),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

CREATE TABLE Pet (
    PetID INT identity PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
	Story VARCHAR(500) NOT NULL,
	GenderID INT,
    TypeID INT,
    UserID INT,
    BreedID INT,
    LocationID INT,
	StatusID INT,
    Age INT,
    [Weight] DECIMAL(5,2),
    [Image] VARCHAR(255),
	FOREIGN KEY (GenderID) REFERENCES Gender(GenderID),
    FOREIGN KEY (TypeID) REFERENCES [Type](TypeID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (BreedID) REFERENCES Breed(BreedID),
    FOREIGN KEY (LocationID) REFERENCES Location(LocationID),
	FOREIGN KEY (StatusID) REFERENCES [Status](StatusID)
);

CREATE TABLE Adoption (
    AdoptionID INT identity(1,1) PRIMARY KEY,
    UserID INT,
	PetID INT,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
	FOREIGN KEY (PetID) REFERENCES Pet(PetID)
);

INSERT INTO [User] ([Name], PhoneNumber)
VALUES
    ('John Doe', '123-456-7890'),
    ('Jane Smith', '987-654-3210');

INSERT INTO [Location] ([Name])
VALUES
    ('Gauteng'),
	('Free State'),
	('Mpumalanga'),
    ('KwaZulu-Natal');

INSERT INTO [Type] ([Name])
VALUES
    ('Dog'),
    ('Cat'),
    ('Bird');

INSERT INTO Breed ([Name], TypeID)
VALUES
    ('Labrador', 1),
    ('Siamese', 2),
    ('Parrot', 3);

INSERT INTO Gender ([Name])
VALUES
    ('Male'),
    ('Female'),
    ('Other');

INSERT INTO [Status] ([Name])
VALUES
    ('Available'),
    ('Adopted');

INSERT INTO Pet ([Name], Story, GenderID, TypeID, UserID, BreedID, LocationID, StatusID, Age, [Weight], [Image])
VALUES
    ('Snoopy', 'A friendly dog looking for a home', 1, 1, 1, 1, 1, 1, 3, 25.5, 'Snoopy.jpg'),
    ('Crisp', 'A playful cat ready for adoption', 2, 2, 2, 2, 2, 2, 2, 9.0, 'Crisp.jpeg'),
	('Donut', 'A naughty dog ready to play around', 3, 3, 2, 2, 2, 2, 2, 13.0, 'donut.jpeg'),
	('Pop Smoke', 'A sleepy dog ready to be cuddled', 2, 2, 2, 2, 2, 2, 2, 9.0, 'pop smoke.jpeg'),
	('Crisp', 'A playful cat ready for adoption', 2, 2, 2, 2, 2, 2, 2, 9.0, 'Crisp.jpeg');



INSERT INTO Donation (UserID, Amount)
VALUES
    (1, 55.00),
    (2, 175.00);

INSERT INTO Adoption (UserID, PetID)
VALUES
    (1, 2);

	SELECT * FROM Pet