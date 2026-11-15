using Microsoft.EntityFrameworkCore;
using saxafon.Data;
using saxafon.DTO;
using saxafon.Models;

namespace saxafon.Services
{


    public interface IResourcesService
    {
    }
    public class ResourcesService : IResourcesService
    {


        private readonly AppDbContext context;

        public ResourcesService(AppDbContext _context)
        {
            context = _context;
        }


        private static ResourcesFullDTO MapToFullDTO(Resources r) {

            var eq = r as Equipments;
            var rm = r as Rooms;
            return new ResourcesFullDTO(
            r.Id,
            r.resource_name,
            r?.ParentId,
            r.status,
            eq?.type,
            r.is_approval_needed,
            r.created_at,
            r.updated_at,
            r?.deleted_at,
            eq?.equipment_model,
            eq?.description,
            eq?.serial_number,
            eq?.inventory_code,
            rm?.capacity,
            rm?.buildingId

            );
        
        }

        private static ResourcesMinimalDTO MapToMinimalDTO(Resources r)
        {
            var eq = r as Equipments;
            var rm = r as Rooms;
            return new ResourcesMinimalDTO(
            r.Id,
            r.resource_name,
            r?.ParentId,
            r.status,
            r.is_approval_needed,
            eq?.equipment_model,
            eq?.description,
            eq?.serial_number,
            eq?.inventory_code,
            rm?.capacity,
            rm?.buildingId
            );
        }   
        public async Task<IEnumerable<ResourcesFullDTO>> GetAllResources()
        {
            //ACTIVA and INACTIVE equipment 
    
            var entities =  await context.Resources.Where(e => e.deleted_at == null).ToListAsync();
            return entities.Select(MapToFullDTO);
        }

        public async Task<IEnumerable<ResourcesMinimalDTO>> GetAllAvaiableResources(int limit = -1 )
        {
            //only ACTIVE  equipment

            var query  =  context.Resources.Where(e => e.deleted_at == null && e.status == Status.ACTIVE);
            if (limit > 0)
            {
                query = query.Take(limit);
            }

            await  query.ToListAsync();
            return query.Select(MapToMinimalDTO);

        }

        public async Task<ResourcesMinimalDTO> GetResourcesById(int id)
        {
            var resource = await context.Resources.FindAsync(id);
            return resource == null ? null : MapToMinimalDTO(resource);
        }

        public async Task<ResourcesMinimalDTO> CreateRoom(ResourcesRoomCreateDTO roomCreateDTO)
        { 
            var room = new Rooms
            {
                resource_name = roomCreateDTO.resource_name,
                status = roomCreateDTO.status,
                is_approval_needed = roomCreateDTO.is_approval_needed,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                capacity = roomCreateDTO.capacity,
                buildingId = roomCreateDTO.buildingId
            };
            context.Resources.Add(room);
            await context.SaveChangesAsync();
            return new  ResourcesMinimalDTO(
                room.Id,
                room.resource_name,
                null,
                room.status,
                room.is_approval_needed,
                null,
                null,
                null,
                null,
                room.capacity,
                room.buildingId
            );
        }
        public async Task<Equipments> CreateEquipment(ResourcesEquipmentCreateDTO createEquipment ) {

            var equipment = new Equipments()
            {
                resource_name = createEquipment.resource_name,
                ParentId = createEquipment.ParentId,
                status = createEquipment.status,
                is_approval_needed = createEquipment.is_approval_needed,
                equipment_model = createEquipment.equipment_model,
                description = createEquipment.description,
                serial_number = createEquipment.serial_number,
                inventory_code = createEquipment.inventory_code,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
                
            };
            context.Resources.Add(equipment);
            await context.SaveChangesAsync();
            return equipment;
        }










    }
}
