namespace ShootingRange.Core {
internal class ScreenBorderTouchState {
    public bool leftTouched = false;
    public bool rightTouched = false;
    public bool upperTouched = false;
    public bool lowerTouched = false;
    public bool frontTouched = false;
    public bool backTouched = false;

    public bool isTouching => leftTouched || rightTouched ||
                              upperTouched || lowerTouched ||
                              frontTouched || backTouched;

    public static bool operator==(ScreenBorderTouchState lhs, ScreenBorderTouchState rhs) {
        return lhs.leftTouched == rhs.leftTouched && lhs.rightTouched == rhs.rightTouched &&
                lhs.upperTouched == rhs.upperTouched && lhs.lowerTouched == rhs.lowerTouched &&
                lhs.frontTouched == rhs.frontTouched && lhs.backTouched == rhs.backTouched;
    }

    public static bool operator !=(ScreenBorderTouchState lhs, ScreenBorderTouchState rhs) {
        return !(lhs == rhs);
    }
}
}
