# Pitch-A-Mon API Spec

## Overview
This API provides Pokémon cry data for the original 151 Pokémon

## Authorization
All endpoints require a **Bearer Token**

## Endpoints
### GET All Pokémon
`GET /pokemon`: returns all cry data for all 151 Pokémon
#### **Parameters:**
- None
#### **Responses:**
-  `200 OK`
```
{
  "pokemon": [
    {
      "name": "Pikachu",
      "cry": 025.wav
    },
    {
      "name": "Charmander",
      "cry": 004.wav
    },
    {
      "name": "Bulbasaur",
      "cry": 001.wav
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
### GET Individual Pokémon 
`GET /pokemon/{name}`: returns cry data for a specfic Pokémon
#### **Parameters:**
    - `name` (string, required)
    - Example: `GET /pokemon/pikachu`
#### **Responses:**
-  `200 OK`
```
{
  "name": "Pikachu",
  "cry": 025.wav
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
    "cry": 025.wav
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
this is a test

