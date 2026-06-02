namespace TodoList.StaticData;

public record CategoryColor(
    int Id,
    string Name,
    string HexColor
);

public static class CategoryColors
{
    public static readonly List<CategoryColor> Colors =
    [
        new(1, "آبی", "#3B82F6"),
        new(2, "بنفش", "#8B5CF6"),
        new(3, "صورتی", "#EC4899"),
        new(4, "قرمز", "#EF4444"),
        new(5, "نارنجی", "#F97316"),
        new(6, "زرد", "#F59E0B"),
        new(7, "لیمویی", "#84CC16"),
        new(8, "سبز", "#22C55E"),
        new(9, "فیروزه‌ای", "#14B8A6"),
        new(10, "آبی آسمانی", "#0EA5E9"),
        new(11, "سرمه‌ای", "#1D4ED8"),
        new(12, "بنفش تیره", "#7C3AED"),
        new(13, "ارغوانی", "#9333EA"),
        new(14, "صورتی تیره", "#DB2777"),
        new(15, "قرمز تیره", "#B91C1C"),
        new(16, "نارنجی تیره", "#C2410C"),
        new(17, "طلایی", "#CA8A04"),
        new(18, "سبز جنگلی", "#15803D"),
        new(19, "سبز دریایی", "#0F766E"),
        new(20, "آبی نفتی", "#0369A1"),
        new(21, "لاجوردی", "#1E40AF"),
        new(22, "یاسی", "#A855F7"),
        new(23, "صورتی روشن", "#F472B6"),
        new(24, "خاکستری", "#64748B")
    ];
}
