using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Repository
{
    //ugovor s app da cemo nekad ovo implementirati!!
    public interface IBillingRepository<Entity>//interfejs nekakvog entiteta, da ne bi morali za svaki objekt pisati metoda -> umjesto <Entity> ide ime domenske klase
    {
        //sta nam treba kad pravimo layer izmedju DBa i kontrolera :
        //treba jedan slog
        Entity Get(int id);
        //citav dataset iz DBa, mozda cemo nekad htjeti da filtriramo pa zato je IQueryable-mozemo detaljnije
        //filtrirati i editovati
        IQueryable<Entity> Get();

        void Insert(Entity entity);
        void Update(Entity entity, int id);
        void Delete(int id);

        bool Commit();//da se spasi sve sto smo promjenili s ovim gore metodama


    }
}
