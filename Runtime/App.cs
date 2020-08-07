using UnityEngine;

namespace Rish
{
    public abstract class App : ScriptableObject
    {
        public abstract DOM Render(Rish rish);
    }
}