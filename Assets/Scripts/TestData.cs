using NuclearBand;
using UnityEngine;

#nullable enable

[CreateAssetMenu(fileName = "FILENAME")]
public class TestData : DataNode
{
    [SerializeField] private string _path;

    [SerializeField] private int _order;
    [SerializeField] private string _description;

    public int Order => _order;
    public string Description => _description;
    public string Path => _path;
}