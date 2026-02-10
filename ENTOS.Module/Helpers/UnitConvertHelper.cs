using System;
using System.Collections.Generic;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper chuyển đổi đơn vị đo lường cho ERP: Hỗ trợ chuyển đổi độ dài, khối lượng, diện tích, thể tích, tốc độ, thời gian, nhiệt độ, tiền tệ, áp suất, năng lượng, công suất, dung lượng dữ liệu, góc, tiêu hao nhiên liệu, tần số, lực, mật độ, lưu lượng.
    /// Tất cả các hàm đều có chú thích tiếng Việt, chỉ sử dụng thư viện .NET chuẩn, dễ mở rộng và bảo trì.
    /// </summary>
    public static class UnitConvertHelper
    {
        private static readonly Dictionary<string, double> LengthFactors = new()
        {
            {"m", 1}, {"cm", 0.01}, {"mm", 0.001}, {"km", 1000}, {"in", 0.0254}, {"ft", 0.3048}, {"yd", 0.9144}, {"mi", 1609.344}
        };
        private static readonly Dictionary<string, double> WeightFactors = new()
        {
            {"kg", 1}, {"g", 0.001}, {"mg", 0.000001}, {"lb", 0.45359237}, {"oz", 0.0283495231}
        };
        private static readonly Dictionary<string, double> AreaFactors = new()
        {
            {"m2", 1}, {"cm2", 0.0001}, {"mm2", 0.000001}, {"km2", 1_000_000}, {"ft2", 0.092903}, {"yd2", 0.836127}, {"mi2", 2_589_988.11}, {"ac", 4046.86}, {"ha", 10000}
        };
        private static readonly Dictionary<string, double> VolumeFactors = new()
        {
            {"m3", 1}, {"cm3", 0.000001}, {"mm3", 0.000000001}, {"l", 0.001}, {"ml", 0.000001}, {"ft3", 0.0283168}, {"in3", 0.0000163871}, {"gal", 0.00378541}
        };
        private static readonly Dictionary<string, double> SpeedFactors = new()
        {
            {"mps", 1}, {"kmh", 0.277778}, {"mph", 0.44704}, {"knot", 0.514444}
        };
        private static readonly Dictionary<string, double> TimeFactors = new()
        {
            {"s", 1}, {"min", 60}, {"h", 3600}, {"day", 86400}
        };
        private static readonly Dictionary<string, double> PressureFactors = new()
        {
            {"pa", 1}, {"kpa", 1000}, {"mpa", 1_000_000}, {"bar", 100_000}, {"psi", 6894.76}, {"atm", 101325}
        };
        private static readonly Dictionary<string, double> EnergyFactors = new()
        {
            {"j", 1}, {"kj", 1000}, {"mj", 1_000_000}, {"wh", 3600}, {"kwh", 3_600_000}, {"cal", 4.184}, {"kcal", 4184}
        };
        private static readonly Dictionary<string, double> PowerFactors = new()
        {
            {"w", 1}, {"kw", 1000}, {"mw", 1_000_000}, {"hp", 745.7}
        };
        private static readonly Dictionary<string, double> DataSizeFactors = new()
        {
            {"b", 1}, {"kb", 1024}, {"mb", 1_048_576}, {"gb", 1_073_741_824}, {"tb", 1_099_511_627_776}
        };
        private static readonly Dictionary<string, double> AngleFactors = new()
        {
            {"deg", 1}, {"rad", 57.2957795}, {"grad", 0.9}
        };
        private static readonly Dictionary<string, double> FuelEconomyFactors = new()
        {
            {"kmpl", 1}, {"l100km", 100}, {"mpg", 0.425144}
        };
        private static readonly Dictionary<string, double> FrequencyFactors = new()
        {
            {"hz", 1}, {"khz", 1000}, {"mhz", 1_000_000}, {"ghz", 1_000_000_000}
        };
        private static readonly Dictionary<string, double> ForceFactors = new()
        {
            {"n", 1}, {"kn", 1000}, {"kgf", 9.80665}, {"lbf", 4.44822}
        };
        private static readonly Dictionary<string, double> DensityFactors = new()
        {
            {"kgm3", 1}, {"gcm3", 1000}, {"lbft3", 16.0185}
        };
        private static readonly Dictionary<string, double> FlowRateFactors = new()
        {
            {"m3s", 1}, {"lmin", 0.0166667}, {"lhr", 0.000277778}, {"gpm", 0.00378541}
        };

        /// <summary>
        /// Chuyển đổi độ dài giữa các đơn vị: mét (m), centimet (cm), milimet (mm), kilomet (km), inch (in), feet (ft), yard (yd), mile (mi).
        /// </summary>
        public static double ConvertLength(double value, string from, string to)
        {
            return value * LengthFactors[from] / LengthFactors[to];
        }
        /// <summary>
        /// Chuyển đổi khối lượng giữa các đơn vị: kilogram (kg), gram (g), miligram (mg), pound (lb), ounce (oz).
        /// </summary>
        public static double ConvertWeight(double value, string from, string to)
        {
            return value * WeightFactors[from] / WeightFactors[to];
        }
        /// <summary>
        /// Chuyển đổi diện tích giữa các đơn vị: mét vuông (m2), centimet vuông (cm2), milimet vuông (mm2), kilomet vuông (km2), feet vuông (ft2), yard vuông (yd2), mile vuông (mi2), acre (ac), hecta (ha).
        /// </summary>
        public static double ConvertArea(double value, string from, string to)
        {
            return value * AreaFactors[from] / AreaFactors[to];
        }
        /// <summary>
        /// Chuyển đổi thể tích giữa các đơn vị: mét khối (m3), centimet khối (cm3), milimet khối (mm3), lít (l), mililít (ml), feet khối (ft3), inch khối (in3), gallon (gal).
        /// </summary>
        public static double ConvertVolume(double value, string from, string to)
        {
            return value * VolumeFactors[from] / VolumeFactors[to];
        }
        /// <summary>
        /// Chuyển đổi tốc độ giữa các đơn vị: mét/giây (mps), kilomet/giờ (kmh), mile/giờ (mph), knot (knot).
        /// </summary>
        public static double ConvertSpeed(double value, string from, string to)
        {
            return value * SpeedFactors[from] / SpeedFactors[to];
        }
        /// <summary>
        /// Chuyển đổi thời gian giữa các đơn vị: giây (s), phút (min), giờ (h), ngày (day).
        /// </summary>
        public static double ConvertTime(double value, string from, string to)
        {
            return value * TimeFactors[from] / TimeFactors[to];
        }
        /// <summary>
        /// Chuyển đổi nhiệt độ giữa các đơn vị: Celsius (C), Fahrenheit (F), Kelvin (K).
        /// </summary>
        public static double ConvertTemperature(double value, string from, string to)
        {
            if (from == to) return value;
            // Celsius, Fahrenheit, Kelvin
            if (from == "C")
            {
                if (to == "F") return value * 9 / 5 + 32;
                if (to == "K") return value + 273.15;
            }
            if (from == "F")
            {
                if (to == "C") return (value - 32) * 5 / 9;
                if (to == "K") return (value - 32) * 5 / 9 + 273.15;
            }
            if (from == "K")
            {
                if (to == "C") return value - 273.15;
                if (to == "F") return (value - 273.15) * 9 / 5 + 32;
            }
            throw new ArgumentException("Đơn vị nhiệt độ không hợp lệ");
        }
        /// <summary>
        /// Chuyển đổi tiền tệ theo tỉ giá truyền vào (giá trị, tỉ giá nguồn, tỉ giá đích).
        /// </summary>
        public static double ConvertCurrency(double value, double fromRate, double toRate)
        {
            return value * fromRate / toRate;
        }
        /// <summary>
        /// Chuyển đổi áp suất giữa các đơn vị: Pascal (pa), Kilopascal (kpa), Megapascal (mpa), Bar (bar), Psi (psi), Atmosphere (atm).
        /// </summary>
        public static double ConvertPressure(double value, string from, string to)
        {
            return value * PressureFactors[from] / PressureFactors[to];
        }
        /// <summary>
        /// Chuyển đổi năng lượng giữa các đơn vị: Joule (j), Kilojoule (kj), Megajoule (mj), Watt-giờ (wh), Kilowatt-giờ (kwh), Calo (cal), Kilocalo (kcal).
        /// </summary>
        public static double ConvertEnergy(double value, string from, string to)
        {
            return value * EnergyFactors[from] / EnergyFactors[to];
        }
        /// <summary>
        /// Chuyển đổi công suất giữa các đơn vị: Watt (w), Kilowatt (kw), Megawatt (mw), Mã lực (hp).
        /// </summary>
        public static double ConvertPower(double value, string from, string to)
        {
            return value * PowerFactors[from] / PowerFactors[to];
        }
        /// <summary>
        /// Chuyển đổi dung lượng dữ liệu giữa các đơn vị: Byte (b), Kilobyte (kb), Megabyte (mb), Gigabyte (gb), Terabyte (tb).
        /// </summary>
        public static double ConvertDataSize(double value, string from, string to)
        {
            return value * DataSizeFactors[from] / DataSizeFactors[to];
        }
        /// <summary>
        /// Chuyển đổi góc giữa các đơn vị: Độ (deg), Radian (rad), Grad (grad).
        /// </summary>
        public static double ConvertAngle(double value, string from, string to)
        {
            return value * AngleFactors[from] / AngleFactors[to];
        }
        /// <summary>
        /// Chuyển đổi tiêu hao nhiên liệu giữa các đơn vị: km/l, l/100km, mpg.
        /// </summary>
        public static double ConvertFuelEconomy(double value, string from, string to)
        {
            return value * FuelEconomyFactors[from] / FuelEconomyFactors[to];
        }
        /// <summary>
        /// Chuyển đổi tần số giữa các đơn vị: Hertz (hz), Kilohertz (khz), Megahertz (mhz), Gigahertz (ghz).
        /// </summary>
        public static double ConvertFrequency(double value, string from, string to)
        {
            return value * FrequencyFactors[from] / FrequencyFactors[to];
        }
        /// <summary>
        /// Chuyển đổi lực giữa các đơn vị: Newton (n), Kilonewton (kn), Kilogram-lực (kgf), Pound-lực (lbf).
        /// </summary>
        public static double ConvertForce(double value, string from, string to)
        {
            return value * ForceFactors[from] / ForceFactors[to];
        }
        /// <summary>
        /// Chuyển đổi mật độ giữa các đơn vị: kg/m3, g/cm3, lb/ft3.
        /// </summary>
        public static double ConvertDensity(double value, string from, string to)
        {
            return value * DensityFactors[from] / DensityFactors[to];
        }
        /// <summary>
        /// Chuyển đổi lưu lượng giữa các đơn vị: mét khối/giây (m3s), lít/phút (lmin), lít/giờ (lhr), gallon/phút (gpm).
        /// </summary>
        public static double ConvertFlowRate(double value, string from, string to)
        {
            return value * FlowRateFactors[from] / FlowRateFactors[to];
        }
    }
} 