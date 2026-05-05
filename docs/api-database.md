# API Database Spec

## Overview
This database stores Pokémon names and their corresponding cry data for the original 151.

## Table: `pokemon`
### Description
Stores pokemon names and corresponding cry file references

## Schema
| Field | Type    | Description                              |
|-------|---------|------------------------------------------|
| id    | integer | unique identifier for each Pokémon       |
| name  | string  | Pokémon name                             |
| cry   | string  | filename of the Pokémon's cry audio file |

## Example Data
| id  | name      | cry     |
|-----|-----------|---------|
| 1   | Bulbasaur | 001.wav |
| 2   | Ivysaur   | 002.wav |
| 3   | Venusaur  | 003.wav |
| ... | ...       | ...     |
| 151 | Mew       | 151.wav | 

## SQL Definition
```sql
CREATE TABLE pokemon (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    cry TEXT NOT NULL
);
```

## Notes 
- `cry` field represents a filename, not a URL or path
- Database is pre-seeded, but can be updated with `POST /pokemon`

