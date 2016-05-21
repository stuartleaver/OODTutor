$(window).load(function () {
function resize() { $('#notifications').height(window.innerHeight - 50); }
    $(window).resize(function () { resize(); });
    resize();

    function refresh_close() {
        $('.close').click(function () { $(this).parent().fadeOut(200); });
    }
    refresh_close();

    var achievementName = $(".AchievementAwardedNameIndicator").val();
    var bottom_center = '<div id="notifications-bottom-right-tab"><div id="notifications-bottom-right-tab-close" class="close"><i class="fa fa-times"></i></span></div><div id="notifications-bottom-right-tab-avatar"><img src="/Content/Images/achievementalert.png" width="70" height="70" /></div><div id="notifications-bottom-right-tab-right"><div id="notifications-bottom-right-tab-right-title"><span>Achievement</span> awarded</div><div id="notifications-bottom-right-tab-right-text">' + achievementName + '</div></div></div>';

    $(document).ready(function () {
        if ($(".AchievementAwardedIndicator").val() === "1") {
            $("#notifications-bottom-right").html();
            $("#notifications-bottom-right").html(bottom_center);
            $("#notifications-bottom-right-tab").addClass("animated bounceInDown");
            refresh_close();
            $(".AchievementAwardedIndicator").val("0");
        }
    });
});