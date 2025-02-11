using System.Text.Json.Serialization;

namespace userPermissionApi.Models;

public partial class TipoPermiso
{
    public TipoPermiso()
    {
        Permisos = new HashSet<Permiso>();
    }
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
