using System.Text.Json.Serialization;

namespace userPermissionApi.Models;

public partial class Permiso
{
    public int id { get; set; }

    public string nombreEmpleado { get; set; } = null!;

    public string apellidoEmpleado { get; set; } = null!;

    public int tipoPermiso { get; set; }

    public DateOnly fechaPermiso { get; set; }


    [JsonIgnore]
    public virtual TipoPermiso? oTipoPermiso { get; set; } 
}
