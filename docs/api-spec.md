# Pitch-A-Mon API Spec

## Overview
This API provides Pokémon cry data for the original 151 Pokémon

## Authorization
All endpoints require a **Bearer Token**

## Endpoints
`GET /pokemon/{name}`: returns cry data for a specfic Pokémon
### **Parameters:**
    - `name` (string, required)
    - Example: `GET /pokemon/pikachu`
### **Responses:**
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

`GET /pokemon/`: returns all cry data for all Pokémon
### **Parameters:**
- None
### **Responses:**
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
## Database
| Field | Type    | Description                              |
|-------|---------|------------------------------------------|
| id    | integer | unique identifier for each Pokémon       |
| name  | string  | Pokémon name                             |
| cry   | string  | filename of the Pokémon's cry audio file |

