

namespace Shared.DTO
{
    public record ResourcesFullDTO(

            int Id,
            string resource_name,
            int? ParentId,
            Status status,
            string? type,
            bool is_approval_needed,
            DateTime created_at,
            DateTime updated_at,
            DateTime ?deleted_at,
            string ?equipment_model,
            string? description,
            string ?serial_number,
            string ?inventory_code,
            int ?capacity,
            string ?buildingId
        );


    public record ResourcesMinimalDTO(
            int Id,
            string resource_name,
            int? ParentId,
            Status status,
            bool? is_approval_needed,
            string? equipment_model,
            string? description,
            string? serial_number,
            string? inventory_code,
            int? capacity,
            string? buildingId
     );

    public record ResourcesRoomCreateDTO(

            string resource_name,
            Status status,
            bool is_approval_needed,
            string description,
            int capacity,
            string buildingId

    );

    
    public record ResourcesEquipmentCreateDTO(
          
            string resource_name,
            int ParentId,
            Status status,
            bool is_approval_needed,
            string equipment_model,
            string description,
            string serial_number,
            string inventory_code
        );

}
