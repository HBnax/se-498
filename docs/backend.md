# Pitch-A-Mon Backend Spec

## Overview
This backend system manages Pokemon cry data for the original 151 Pokemon.  
It handles authenticated requests, processes business logic, interacts with the database, and returns structured responses.

---

## Authorization
All endpoints require a **Bearer Token**

---

## Architecture
```
Client
  ↓ HTTP Request
Backend Server
  ↓
Service Layer (Validation & Logic)
  ↓
Database (Pokemon Table)
```

---

## Data Model

### Pokemon Object
```
{
  "id": int,
  "name": string,
  "cry_url": string, C:/Path/To/Cry/Audio/File
}
```

---

## Database Schema

### Table: pokemon

| Column | Type | Constraints |
|------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT |
| name | VARCHAR | UNIQUE, NOT NULL |
| cry | VARCHAR | NOT NULL |

---

## Processing Flow

### GET /pokemon
```
1. Receive request
2. Validate Bearer Token
3. Query all Pokemon from database
4. Return list of Pokemon
```

---

### GET /pokemon/{name}
```
1. Receive request
2. Validate Bearer Token
3. Query Pokemon by name
4. If not found → return 404
5. Return Pokemon data
```

---

### POST /pokemon
```
1. Receive request
2. Validate Bearer Token
3. Validate request body (name, cry)
4. Check if Pokemon already exists
   → If exists → return 409
5. Insert new Pokemon into database
6. Return success response (201)
```

---

## Validation Rules

- `name` must be a non-empty string
- `cry` must be a valid string (file name format)
- Duplicate Pokemon names are not allowed

---

## Error Handling

| Status Code | Description               |
|------|---------------------------|
| 400 | Missing or invalid fields |
| 401 | Invalid or missing token  |
| 404 | Pokemon not found         |
| 409 | Pokemon already exists    |
| 500 | Internal server error     |

---

## Security

- All endpoints require Bearer Token authentication
- Input validation enforced on all requests
- Prevent duplicate and malformed data insertion

---

## Future Improvements

- Add PUT endpoint for updates
- Add DELETE endpoint
- Implement pagination
- Store cry files in cloud storage (e.g., AWS S3)
- Add caching layer (e.g., Redis)  