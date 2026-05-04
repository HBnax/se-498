# 151 Pokémon API Spec

## Overview
This API provides Pokémon cry data for the original 151 Pokémon

## Headers
- **Authorization:** Bearer {token} 
- **Content Type:** application/json

## Base URL
https://localhost:8080

## Authentication
All endpoints require a valid Bearer token.

### Setup
Create an `.env` file in the root directory:
- `AUTH_BEARER_TOKEN=<your_token_here>` (can be any string you want)


## Endpoints
### `GET /pokemon`
Returns cry data for all supported Pokémon
#### **Responses:**
-  `200 OK`
```
{
  "pokemon": [
    {
      "id": 025,
      "name": "Pikachu",
      "cry": "025.wav"
    },
    {
      "id": 4,
      "name": "Charmander",
      "cry": "004.wav"
    },
    {
      "id": 1,
      "name": "Bulbasaur",
      "cry": "001.wav"
    },
    ...
  ]
}
```
- `401 Unauthorized`
```
{
  "error": "Invalid or missing token"
}
```
### `GET /pokemon/{name}`
Returns cry data for a specific Pokémon
#### **Parameters:**
    - name (string, required)
Example: `GET /pokemon/pikachu`
#### **Responses:**
-  `200 OK`
```
{
  "id": 025,
  "name": "Pikachu",
  "cry": "025.wav"
}
```
- `404 Not Found`
```
{
  "error": "Pokémon not found"
}
```
- `401 Unauthorized`
```
{
  "error": "Invalid or missing token"
}
```
### `GET /pokemon/{name}/cry`
Returns the actual Pokémon cry audio file
#### **Parameters:**
    - name (string, required)
Example: `GET /pokemon/pikachu/cry`
#### **Responses:**
- `200 OK`
- Content-Type: `audio/wav`
- Returns downloadable audio file

Response headers:
```
 content-disposition: attachment; filename=025.wav; filename*=UTF-8''025.wav 
 content-length: 78542 
 content-type: audio/wav 
```
- `404 Not Found`
```
{
  "error": "Pokémon not found"
}
```
- `401 Unauthorized`
```
{
  "error": "Invalid or missing token"
}
```
### POST
`POST /pokemon`: adds a new Pokémon cry entry to the database
#### **Request Body**
```
{
    "name": string (required),
    "cry": string (required)
}
```
#### **Response**
- `201 Created`
```
{
  "message": "Pokémon added successfully",
  "pokemon": {
    "id": 025,
    "name": "Pikachu",
    "cry": "025.wav"
  }
}
```
- `400 Bad Request`
```
{
  "error": "Missing required fields"
}
```
- `401 Unauthorized`
```
{
    "error": "Invalid or missing token"
}
```
- `409 Conflict`
```
{
    "error": "Pokémon already exists"
}
```

### Integration Notes
- Pokémon Names are case-insensitive
- Cry files are returned as .wav

##
