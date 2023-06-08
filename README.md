# Solar Coffee

Productive Dev Solar Coffee Project

Creating database:
CREATE DATABASE "solardev";
izvrsiti migracije: cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffee.Web/ database update
INSERT INTO "Products" ("CreatedOn", "UpdatedOn", "Name", "Description", "Price", "IsTaxable", "IsArchived")
VALUES
(NOW(), NOW(), '5 KG Dark Roast', '5 KG bag of dark roast coffee beans', 100, true, false),
(NOW(), NOW(), '10 KG Dark Roast', '10 KG bag of dark roast coffee beans', 280, true, false),
(NOW(), NOW(), '25 KG Dark Roast', '25 KGbag of dark roast coffee beans', 450, true, false),
(NOW(), NOW(), '5 KG Light Roast', '5 KG bag of light roast coffee beans', 100, true, false),
(NOW(), NOW(), '10 KG Light Roast', '10 KG bag of light roast coffee beans', 280, true, false),
(NOW(), NOW(), '25 KG Light Roast', '25 KG bag of light roast coffee beans', 450, true, false);

INSERT INTO "ProductInventories" ("CreatedOn", "UpdatedOn", "QuantityOnHand", "IdealQuantity", "ProductId")
VALUES
(NOW(), NOW(), 20, 24, 19),
(NOW(), NOW(), 12, 20, 20),
(NOW(), NOW(), 16, 20, 21),
(NOW(), NOW(), 9, 12, 22),
(NOW(), NOW(), 24, 12, 23),
(NOW(), NOW(), 0, 8, 24);

INSERT INTO "CustomerAddresses" ("CreatedOn", "UpdatedOn", "AddressLine1", "AddressLine2", "City", "State", "PostalCode", "Country")
VALUES
(NOW(), NOW(), 'Bulevar Kralja Aleksandra 2', null, 'Beograd', '', 11000, 'Serbia'),
(NOW(), NOW(), 'Lenjinova 120', null, 'Beograd', '', 11000, 'Serbia'),
(NOW(), NOW(), 'Bulevar Kralja Aleksandra 269', 'Banovo Brdo 12a', 'Beograd', '', 11000, 'Serbia'),
(NOW(), NOW(), 'Zarka Zrenjanina 99', null, 'Pancevo', '', 26000, 'Serbia'),
(NOW(), NOW(), 'One way street 12b', null, 'London', '', 31221, 'United Kingdom'),
(NOW(), NOW(), 'Estana balana 224', null, 'Madrid', '', 44433, 'Spain');

INSERT INTO "Customers" ("CreatedOn", "UpdatedOn", "FirstName", "LastName", "PrimaryAddressId")
VALUES
(NOW(), NOW(), 'Adam', 'Black', 1),
(NOW(), NOW(), 'Zika', 'Zikic', 2),
(NOW(), NOW(), 'Pera', 'Peric', 3),
(NOW(), NOW(), 'Steph', 'Wide', 4),
(NOW(), NOW(), 'Brook', 'White', 5),
(NOW(), NOW(), 'Sergio', 'Escobedo', 6);

_Node js verzija 14.17.0_
