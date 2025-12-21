\connect elitegaertner_test

CREATE TABLE PROFILE (
    ProfileId SERIAL PRIMARY KEY,
    ProfilePictureUrl TEXT,
    UserName TEXT NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    EMail TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    PhoneNumber TEXT NOT NULL,
    ProfileText TEXT NOT NULL,
    ShareMail BOOLEAN NOT NULL,
    SharePhoneNumber BOOLEAN NOT NULL,
    UserCreated TIMESTAMPTZ NOT NULL
);


CREATE TABLE HARVESTUPLOADS (
    UploadId SERIAL PRIMARY KEY,
    ImageUrl TEXT NOT NULL,
    Description TEXT NOT NULL,
    WeightGramm REAL NOT NULL,
    WidthCm REAL NOT NULL,
    LengthCm REAL NOT NULL,
    UploadDate TIMESTAMPTZ NOT NULL,
    ProfileId INT NOT NULL,
    
    FOREIGN KEY (ProfileId) REFERENCES PROFILE (ProfileId)
);


CREATE TABLE TAGS (
    TagId SERIAL PRIMARY KEY,
    Label TEXT NOT NULL
);


CREATE TABLE PROFILEPREFERENCES (
    TagId INT,
    ProfileId INT,
    DateUpdated TIMESTAMPTZ NOT NULL,
    
    PRIMARY KEY (TagId, ProfileId),
    
    FOREIGN KEY (TagId) REFERENCES TAGS (TagId),
    FOREIGN KEY (ProfileId) REFERENCES PROFILE (ProfileId)
);


CREATE TABLE HARVESTTAGS (
    TagId INT,
    UploadId INT,
    
    PRIMARY KEY (TagId, UploadId),
    
    FOREIGN KEY (TagId) REFERENCES TAGS(TagId),
    FOREIGN KEY (UploadId) REFERENCES HARVESTUPLOADS (UploadId)    
);


CREATE TABLE REPORT (
    ReportId SERIAL PRIMARY KEY,
    Reason TEXT NOT NULL,
    ReportDate TIMESTAMPTZ NOT NULL,
    UploadId INT NOT NULL,
    
    FOREIGN KEY (UploadId) REFERENCES HARVESTUPLOADS(UploadId)
);


CREATE TABLE RATING (
    ContentReceiverId INT,
    ContentCreatorId INT,
    ProfileRating BOOLEAN NOT NULL,
    RatingDate TIMESTAMPTZ NOT NULL,
    
    PRIMARY KEY (ContentReceiverId, ContentCreatorId),

    FOREIGN KEY (ContentCreatorId) REFERENCES PROFILE(ProfileId),
    FOREIGN KEY (ContentReceiverId) REFERENCES PROFILE(ProfileId)
);