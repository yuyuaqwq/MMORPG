using Google.Protobuf.WellKnownTypes;
using QFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMapManagerSystem : ISystem
{
    public int MapId { get; }
    public bool IsInMap { get; }

    public void JoinedMap(int mapId);
    public void ExitMap();
}

public class MapManagerSystem : AbstractSystem, IMapManagerSystem
{
    private int _mapId = 0;
    private bool _isInMap = false;
    public int MapId => _mapId;
    public bool IsInMap => _isInMap;

    public void JoinedMap(int mapId)
    {
        Debug.Assert(!_isInMap);
        Logger.Info("Game", $"加入地图:{mapId}");
        _mapId = mapId;
        this.SendEvent(new JoinedMapEvent(mapId));
    }

    public void ExitMap()
    {
        Debug.Assert(_isInMap);
        Logger.Info("Game", $"退出地图:{_mapId}");
        _mapId = 0;
        _isInMap = false;
        //TODO 发送事件
    }

    protected override void OnInit()
    {
    }
}
