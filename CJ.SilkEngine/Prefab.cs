using CJ.SilkEngine.GameObjects;

namespace CJ.SilkEngine;

public interface IPrefab
{
    public GameObject Instantiate(CJGame game);
}
