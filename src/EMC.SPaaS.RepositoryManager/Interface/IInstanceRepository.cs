﻿using System.Collections.Generic;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public interface IInstanceRepository
    {
        void Create(InstanceEntity instance);
        void Delete(InstanceEntity instance);
        IEnumerable<InstanceEntity> GetInstancesForUser(int userId);
        InstanceEntity GetInstance(int instanceId, int userId);
        void Provision(InstanceEntity instance, UserEntity user);
        void Provision(InstanceEntity instance, int userId);
        void TurnOff(InstanceEntity instance, UserEntity user);
        void TurnOff(InstanceEntity instance, int userId);
        void TurnOn(InstanceEntity instance, UserEntity user);
        void TurnOn(InstanceEntity instance, int userId);
        void Release(InstanceEntity instance, UserEntity user);
        void Release(InstanceEntity instance, int userId);
    }
}