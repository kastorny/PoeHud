using System.Collections.Generic;
using System.Linq;
using PoeHUD.Framework;
using PoeHUD.Poe;
using PoeHUD.Poe.EntityComponents;
using PoeHUD.Poe.Files;
using PoeHUD.Poe.UI;

namespace PoeHUD.Controllers
{
    public class GameController
    {
       

        public GameController(Memory memory)
        {
            Memory = memory;
            Area = new AreaController(this);
            EntityListWrapper = new EntityListWrapper(this);
            Window = new GameWindow(memory.Process);
            Game = new TheGame(memory);
            Files = new FsController(memory);
        }
        public EntityListWrapper EntityListWrapper { get; private set; }
        public GameWindow Window { get; private set; }
        public TheGame Game { get; private set; }
        public AreaController Area { get; private set; }

        public Memory Memory { get; private set; }

        public IEnumerable<EntityWrapper> Entities
        {
            get { return EntityListWrapper.Entities; }
        }

        public EntityWrapper Player
        {
            get { return EntityListWrapper.Player; }
        }

        public bool InGame
        {
            get { return Game.IngameState.InGame; }
        }

        public FsController Files { get; private set; }

        public void RefreshState()
        {
            if (InGame)
            {
                EntityListWrapper.RefreshState();
                Area.RefreshState();
            }
        }

        public List<EntityWrapper> GetAllPlayerMinions()
        {
            return Entities.Where(x => x.HasComponent<Player>()).SelectMany(c => c.Minions).ToList();
        }

        public EntityLabel GetLabelForEntity(EntityWrapper entity)
        {
            var hashSet = new HashSet<int>();
            int entityLabelMap = Game.IngameState.EntityLabelMap;
            int num = entityLabelMap;
            while (true)
            {
                hashSet.Add(num);
                if (Memory.ReadInt(num + 8) == entity.Address)
                {
                    break;
                }
                num = Memory.ReadInt(num);
                if (hashSet.Contains(num) || num == 0 || num == -1)
                {
                    return null;
                }
            }
            return Game.ReadObject<EntityLabel>(num + 12);
        }

        internal EntityWrapper GetEntityById(int id)
        {
            return EntityListWrapper.GetByID(id);
        }
    }
}