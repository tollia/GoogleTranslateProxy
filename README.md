# GoogleTranslateProxy
Access restricted by a compiled in secret that whoever wishing to use this has to know.
The JWT token to place in the Authorization: Bearer <token> can be generated by calling:
  https://host:port/api/Translate/Token?secret=the%20secret

Swagger is present at /swagger and only active in Development mode.

Translate using:
  https://host:port/api/Translate/Translate

  Sample payload to POST to the endpoint is:
  <pre>
  {
    "text": "Computer",
    "sourceLanguage": "en",
    "targetLanguage": "is"
  }
  </pre>
  
Result is returned in JSON format:
  <pre>
  {
    "data": {
      "translations": [
        {
          "translatedText": "T�lva",
          "detectedSourceLanguage": null
        }
      ]
    },
    "error": null
  }
  </pre>
  The call can produce:\
  Status200OK\
  Status400BadRequest

If there is a parameter error like passing targetLanguage=fictionalLanguage we get:
  <pre>
  {
    "data": null,
    "error": {
      "code": 400,
      "message": "Invalid Value",
      "errors": [
        {
          "message": "Invalid Value",
          "domain": "global",
          "reason": "invalid"
        }
      ],
      "details": [
        {
          "@type": "type.googleapis.com/google.rpc.BadRequest",
          "fieldViolations": [
            {
              "field": "target",
              "description": "Target language: fictionalLanguage"
            }
          ]
        }
      ]
    }
  }
  </pre>

  
Please note that the code is set up to take sensitive config from appsettings.Passwords.json
which overrides what is already in appsettings.json. Place your secret and Google apiKey in either location.
