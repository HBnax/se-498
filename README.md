## Project for SE 498: Software Engineering Capstone

### Container Running Instructions:
#### Start Containers (in root folder)
- bash: `podman compose up --build`
- via IDE: run/play button next to `services:`

#### Start Backend API (in `backend` folder)
- In bash, run: 
  - `export PokemonApi__BearerToken=pitchamon`
  - `dotnet run`

#### Accessing Databases
- Backend DB: `podman compose exec backend-db psql -U postgres -d pitchamon`
- API DB: `podman compose exec db psql -U postgres -d pokemon`

#### Accessing Swagger UI
- In Browser 
  - Cry API: `http://localhost:8080/swagger`
  - Backend API: `http://localhost:5090/swagger`

### Contributors:
- Jeffrey Bok
- Halle Broadnax 
- Daniel Min
