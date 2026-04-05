CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE processing_history (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    original_song_file TEXT NOT NULL,
    pokemon_used VARCHAR(100) NOT NULL,
    cry_file_used VARCHAR(255) NOT NULL,
    processed_song_file TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_user
        FOREIGN KEY (user_id) 
        REFERENCES users(id)
        ON DELETE CASCADE
);