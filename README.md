## Project for SE 498: Software Engineering Capstone

### Container Running Instructions:
#### Start Containers (in root folder)
- bash: `podman compose up --build`
- via IDE: run/play button next to `services:`

#### Accessing Databases
- Backend DB: `podman compose exec backend-db psql -U postgres -d pitchamon`
- API DB: `podman compose exec db psql -U postgres -d pokemon`

#### Accessing Swagger UI (after running container)
- In Browser 
  - Cry API: `http://localhost:8080/swagger`
  - Backend API: `http://localhost:8081/swagger`
  - Frontend: `http://localhost:5173`

#### Bearer Token for Pokemon API
- In Root folder, add an `.env` file with:
  - AUTH_BEARER_TOKEN=`<token>` (can be anything you want)

### Contributors:
- Jeffrey Bok
- Halle Broadnax 
- Daniel Min
