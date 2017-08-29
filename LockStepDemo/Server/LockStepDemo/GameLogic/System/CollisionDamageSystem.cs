﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CollisionDamageSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(CollisionComponent),
            typeof(Camp),
            typeof(FlyObjectComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            DamageLogic(list[i]);
        }
    }

    void DamageLogic(EntityBase entity)
    {
        CollisionComponent cc = entity.GetComp<CollisionComponent>();
        FlyObjectComponent fc = entity.GetComp<FlyObjectComponent>();

        if (cc.CollisionList.Count > 0)
        {
            Debug.Log("collision " + cc.CollisionList[0].ID);

            for (int i = 0; i < cc.CollisionList.Count; i++)
            {
                if (cc.CollisionList[i].GetExistComp<LifeComponent>()
                    && fc.createrID != cc.CollisionList[i].ID)
                {
                    FlyDamageLogic(entity, cc.CollisionList[i]);
                }
            }
        }
    }

    void FlyDamageLogic(EntityBase fly, EntityBase entity)
    {
        FlyObjectComponent fc = fly.GetComp<FlyObjectComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();

        lc.life -= fc.damage;
        //派发事件
        ECSEvent.DispatchEvent(CharacterEventType.Damage, entity);
    }
}

public enum CharacterEventType
{
    Init,   //初始化
    Move,   //移动
    Attack, //攻击
    Damage, //受伤
    Recover,//恢复
    Die,    //死亡
    SKill,  //使用技能
    BeBreak,//被打断
    Resurgence, //复活
    EnterArea,  //进入某区域
    ExitArea,   //离开某区域
    Destroy,
}


