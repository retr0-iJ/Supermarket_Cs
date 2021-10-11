CREATE DATABASE marketDB
go
USE marketDB
go

CREATE TABLE MsProduct(
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName VARCHAR(21) NOT NULL,
    ProductPrice INT NOT NULL,
    ProductQty INT NOT NULL,
	IsDeleted TINYINT DEFAULT 0 NOT NULL
)

CREATE TABLE TransactionHeader(
	TransactionID INT PRIMARY KEY IDENTITY,
	PaymentMethod VARCHAR(7) NOT NULL 
)

CREATE TABLE TransactionDetail(
	TransactionID INT REFERENCES TransactionHeader(TransactionID) ON UPDATE CASCADE NOT NULL,
	ProductID INT REFERENCES MsProduct(ProductID) ON UPDATE CASCADE NOT NULL,
	ProductQty INT NOT NULL,
	PRIMARY KEY(TransactionID, ProductID)
)

go
CREATE PROC View_All_Products
AS
BEGIN
    SELECT * FROM MsProduct WHERE IsDeleted = 0
END

go
CREATE PROC Insert_Product
@ProductName VARCHAR(21),
@ProductPrice INT,
@ProductQty INT
AS
BEGIN
    INSERT INTO MsProduct VALUES(@ProductName, @ProductPrice, @ProductQty)
END

go
CREATE PROC Update_Product
@ProductID INT,
@ProductName VARCHAR(21),
@ProductPrice INT,
@ProductQty INT
AS
BEGIN
    UPDATE MsProduct 
        SET ProductName = @ProductName, ProductPrice = @ProductPrice, ProductQty = @ProductQty
    WHERE ProductID = @ProductID
END

go
CREATE PROC Delete_Product
@ProductID INT
AS
BEGIN   
    UPDATE MsProduct SET IsDeleted = 1 WHERE ProductID = @ProductID
END

go
CREATE PROC View_All_Transactions
AS
BEGIN
	SELECT * FROM TransactionHeader
END

go
CREATE PROC View_All_Transaction_Details
@TransactionID INT
AS
BEGIN
	SELECT td.*, mp.ProductName, mp.ProductPrice FROM TransactionDetail td
	JOIN MsProduct mp ON td.ProductID = mp.ProductID
	WHERE td.TransactionID = @TransactionID
END

BEGIN TRAN

go
CREATE PROC Insert_Transaction
@PaymentMethod VARCHAR(7)
AS
BEGIN
	INSERT INTO TransactionHeader OUTPUT Inserted.TransactionID VALUES(@PaymentMethod)
END

go
CREATE PROC Insert_Transaction_Detail
@TransactionID INT,
@ProductID INT,
@ProductQty INT
AS
BEGIN
	INSERT INTO TransactionDetail VALUES(@TransactionID, @ProductID, @ProductQty)
END

go
CREATE TRIGGER Buy_Products
ON TransactionDetail 
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE mp SET mp.ProductQty -= i.ProductQty
	FROM MsProduct mp, inserted i
	WHERE mp.ProductID = i.ProductID
END