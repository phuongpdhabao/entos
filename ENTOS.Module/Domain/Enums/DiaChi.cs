using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DiaChi
    {
					[XafDisplayName("Tý")]
        Mouse,
					[XafDisplayName("Sửu")]
        Buffalo,
					[XafDisplayName("Dần")]
        Tiger,
			[ImageName("DiaChiCat")]		[XafDisplayName("Mão")]
        Cat,
			[ImageName("DiaChiDragon")]		[XafDisplayName("Thìn")]
        Dragon,
			[ImageName("DiaChiSnake")]		[XafDisplayName("Tỵ")]
        Snake,
			[ImageName("DiaChiHorse")]		[XafDisplayName("Ngọ")]
        Horse,
			[ImageName("DiaChiGoat")]		[XafDisplayName("Mùi")]
        Goat,
			[ImageName("DiaChiMonkey")]		[XafDisplayName("Thân")]
        Monkey,
			[ImageName("DiaChiCock")]		[XafDisplayName("Dậu")]
        Cock,
			[ImageName("DiaChiDog")]		[XafDisplayName("Tuất")]
        Dog,
			[ImageName("DiaChiPig")]		[XafDisplayName("Hợi")]
        Pig,
	    }

}