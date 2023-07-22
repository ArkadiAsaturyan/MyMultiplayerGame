using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private JoystickController2 joystickController;
        public override void InstallBindings()
        {
            //Container.Bind<JoystickController2>().FromInstance(joystickController).AsSingle().NonLazy();
            //Container.Bind<JoystickController2>().FromComponentInNewPrefab(joystickController).AsSingle();
            Container.BindInstance(joystickController).AsSingle().NonLazy();


        }
    }
}
