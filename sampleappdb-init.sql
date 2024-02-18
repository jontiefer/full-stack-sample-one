CREATE SCHEMA dbo;

CREATE TABLE IF NOT EXISTS dbo.Clients (
    ClientID SERIAL PRIMARY KEY,
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    Phone VARCHAR(50),
    Email VARCHAR(255)
);

INSERT INTO dbo.Clients (FirstName, LastName, Phone, Email)
VALUES 
('John', 'Lewis', '123-456-7890', 'john.lewis@example.com'),
('Jane', 'Morris', '234-567-8901', 'jane.morris@example.com'),
('Jim', 'Zeal', '345-678-9012', 'jim.zeal@example.com'),
('Jack', 'Rogers', '456-789-0123', 'jack.rogers@example.com'),
('Johnny', 'McLean', '567-890-1234', 'johnny.mclean@example.com'),
('Jill', 'Valentine', '678-901-2345', 'jill.valentine@example.com'),
('Joel', 'Miller', '789-012-3456', 'joel.miller@example.com'),
('Ellie', 'Williams', '890-123-4567', 'ellie.williams@example.com'),
('Sarah', 'Connor', '901-234-5678', 'sarah.connor@example.com'),
('John', 'Rambo', '012-345-6789', 'john.rambo@example.com');
