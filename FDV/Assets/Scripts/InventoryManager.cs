using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 1. El Patrón Singleton
    public static InventoryManager Instance { get; private set; }

    // 2. Almacenamiento: Usamos un HashSet para guardar los items.
    // HashSet es muy rápido para verificar si un item YA existe (Contains/ContainsItem).
    private HashSet<InventoryItem> items = new HashSet<InventoryItem>();

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Si quieres que el inventario persista entre escenas:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // AÑADIR/REGISTRAR UN ÍTEM
    public void AddItem(InventoryItem item)
    {
        if (item != InventoryItem.None && items.Add(item))
        {
            Debug.Log($"[INVENTARIO] Item añadido: {item.ToString()}");
            // Aquí podrías añadir lógica de UI para mostrar el icono.
        }
    }

    // COMPROBAR SI UN ÍTEM EXISTE (REGISTRO)
    public bool ContainsItem(InventoryItem item)
    {
        return items.Contains(item);
    }

    // QUITAR UN ÍTEM (CONSUMIR)
    public void RemoveItem(InventoryItem item)
    {
        if (items.Remove(item))
        {
            Debug.Log($"[INVENTARIO] Item consumido: {item.ToString()}");
            // Aquí podrías añadir lógica de UI para quitar el icono.
        }
    }
    
    // Opcional: Para DEBUG
    public void ListItems()
    {
        string list = "Inventario actual: ";
        foreach (var item in items)
        {
            list += item.ToString() + ", ";
        }
        Debug.Log(list);
    }
}