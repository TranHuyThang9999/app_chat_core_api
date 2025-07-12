CREATE TABLE "User"
(
    Id                 BIGSERIAL PRIMARY KEY,
    NickName           VARCHAR(255),
    Avatar             VARCHAR(255),
    Email              VARCHAR(255) UNIQUE,
    Age                INTEGER,
    PhoneNumber        VARCHAR(20),
    Address            TEXT,
    UserName           VARCHAR(255) UNIQUE NOT NULL,
    PassWord           VARCHAR(255)        NOT NULL,
    LastChangePassWord TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedDate        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tạo function để update timestamp
CREATE
OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt
= CURRENT_TIMESTAMP;
RETURN NEW;
END;
$$
language 'plpgsql';

-- Tạo trigger
CREATE TRIGGER update_user_updated_at
    BEFORE UPDATE
    ON "User"
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();