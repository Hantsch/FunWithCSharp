// connecting to mongodb Server
db = connect("mongodb+srv://<username>:<password>@mytestcluster-m2hjn.mongodb.net/test?retryWrites=true");

// dop collection if exists
db.persons.drop();

// create collection with schema
db.createCollection("persons",
   { validator: {
      $jsonSchema: {
         bsonType: "object",
         required: [ "name", "gender", "address" ],
         properties: {
            name: {
               bsonType: "string",
               description: "required, full name of person"
            },
            gender: {
               enum: [ "male", "female" ],
               description: "required, must be either male or female"
            },
            birthday: {
               bsonType: "int",
               minimum: 1900,
               maximum: 3000,
               exclusiveMaximum: false,
               description: "optional, must be between 1900 and 3000, note: database has to be updated before year 3000"
            },
            email: {
                bsonType : "string",
                pattern : "^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$",
                description: "must be a string and match the regular expression pattern"
            },
            address: {
                bsonType: "object",
                required: [ "street", "city", "zip", "countryCode" ],
                properties: {
                    street: {
                        bsonType: "string"
                    },
                    city: {
                        bsonType: "string"
                    },
                    zip: {
                        bsonType: "string",
                        description: "required, postalcode of where person lives",
                        pattern: "^[A-Z0-9]*$"
                    },
                    countryCode: {
                        bsonType: "string",
                        pattern: "^[A-Z]{2}$"
                    }
                }
            }
         }
      }
   }
});

// insert test record, schould work
db.persons.insertOne(
   {
      name: "dummy mcdummyface",
      gender: "male",
      birthday: NumberInt(1995),
      email: "test@blah.com",
      address: {
          street: "test",
          city: "test",
          zip: "1337",
          countryCode: "AK"
      }
   });
   