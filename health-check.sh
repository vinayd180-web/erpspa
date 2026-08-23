#!/bin/bash
echo "========================================="
echo "  Shree Parth Academy - Health Check"
echo "========================================="
echo ""

# Check if app is running
echo "📱 Checking if app is running..."
if ps aux | grep -v grep | grep "Shivakala.Web" > /dev/null; then
    echo "✅ App is running"
else
    echo "❌ App is NOT running"
    echo "   Start with: dotnet run --project src/Shivakala.Web/Shivakala.Web.csproj"
fi

# Check port 5005
echo ""
echo "🔌 Checking port 5005..."
if netstat -tulpn 2>/dev/null | grep -q ":5005"; then
    echo "✅ Port 5005 is listening"
else
    echo "❌ Port 5005 is NOT listening"
fi

# Check database
echo ""
echo "🗄️  Checking database..."
if [ -f "src/Shivakala.Web/App_Data/shivakala.db" ]; then
    echo "✅ Database exists"
    ls -lh src/Shivakala.Web/App_Data/shivakala.db
else
    echo "⚠️  Database not found (SQLite file)"
fi

# Check logo
echo ""
echo "🖼️  Checking logo..."
if [ -f "src/Shivakala.Web/wwwroot/img/ShivKalaLogo.jpeg" ]; then
    echo "✅ Logo exists"
else
    echo "❌ Logo not found"
fi

# Check PWA icons
echo ""
echo "📱 Checking PWA icons..."
icon_count=$(ls src/Shivakala.Web/wwwroot/img/icon-*.png 2>/dev/null | wc -l)
if [ $icon_count -ge 5 ]; then
    echo "✅ $icon_count PWA icons found"
else
    echo "❌ PWA icons missing (found $icon_count)"
fi

# Check views
echo ""
echo "📄 Checking important views..."
if [ -f "src/Shivakala.Web/Views/Shared/_Layout.cshtml" ]; then
    echo "✅ _Layout.cshtml found"
else
    echo "❌ _Layout.cshtml missing"
fi

# Check git status
echo ""
echo "🔗 Git status..."
git status --short | head -5

echo ""
echo "========================================="
echo "  Health Check Complete"
echo "========================================="
