# Backend Database Spec

## Overview
This database stores user information & each user's song processing history.

## Schema

### Table: `users`
| Field         | Type     | Description                            |
|---------------|----------|----------------------------------------|
| id            | integer  | unique identifier for each user        |
| email         | string   | user email address                     |
| password_hash | string   | hashed user password                   |
| created_at    | datetime | timestamp when the account was created |

### Notes
- `email` should be unique
- `password_hash` should be stored instead of plain password

### Table: `processing_history`
| Field              | Type     | Description                                        |
|--------------------|----------|----------------------------------------------------|
| id                 | integer  | unique identifier for each processed song record   |
| user_id            | int      | reference to user who created the processed song   |
| original_song_file | string   | filename of uploaded original song                 |
| pokemon_used       | string   | name of pokemon used for processing                |
| cry_file_used      | string   | filename returned by the API & used for processing |
| processed_file     | string   | filename of the processed output song              |
| created_at         | datetime | timestamp when the song record was created         |

### Notes
- `user_id` is a foreign key to `users.id`
- `pokemon_used` is stored for display purposes
- `cry_file_used` is stored for reference to exact cry used in processing
- stores completed processing records only (no pending or failed states)

### Relationships
- One user can have many processing history records
- each processing history record belongs to one user
