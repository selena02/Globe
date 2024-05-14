import { useEffect, useState } from "react";
import "./Notifications.scss";
import { Close, Notifications } from "@mui/icons-material";
import fetchAPI from "../../utils/fetchAPI";
import { handleApiErrors } from "../../utils/displayApiErrors";
import Avatar from "../../components/Avatar/Avatar";
import { Link } from "react-router-dom";

interface FollowNotification {
  notificationId: number;
  followerId: number;
  followerUsername: string;
  followerProfilePicture: string | null;
  followedAt: Date;
}

export const NotificationsDropdown = () => {
  const [notifications, setNotifications] = useState<FollowNotification[]>([]);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [count, setCount] = useState(1);
  const [isLoding, setIsLoading] = useState(false);

  const toggleDropdown = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  useEffect(() => {
    fetchNotifications();
    const handleOutsideClick = (event: any) => {
      if (isDropdownOpen && !event.target.closest("#Notifications-button")) {
        setIsDropdownOpen(false);
      }
    };

    if (isDropdownOpen) {
      window.addEventListener("click", handleOutsideClick);
    }

    return () => {
      window.removeEventListener("click", handleOutsideClick);
    };
  }, [isDropdownOpen]);

  const fetchNotifications = async () => {
    try {
      const data = await fetchAPI<{
        followNotifications: FollowNotification[];
      }>("users/notifications", {
        method: "GET",
      });
      setNotifications(data.followNotifications);
      setCount(data.followNotifications.length);
    } catch (error) {
      handleApiErrors(error);
    }
  };

  const removeNotification = async (id: number) => {
    setIsLoading(true);
    try {
      await fetchAPI(`Follows/notification/${id}`, {
        method: "DELETE",
      });
      setNotifications((prevNotifications) =>
        prevNotifications.filter((n) => n.followerId !== id)
      );
      setCount(count - 1);
    } catch (error) {
      handleApiErrors(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div id="notifications-container">
      <button
        title="Show notifications"
        type="button"
        id="Notifications-button"
        onClick={toggleDropdown}
      >
        <Notifications id="notifications-icon" />
      </button>
      <span className={count > 0 ? "count-display" : "none"}>{count}</span>
      <div
        className={`notifications-dropdown-container ${
          isDropdownOpen && notifications.length > 0 ? "dropdown-open" : "none"
        }`}
      >
        {notifications.length > 0 ? (
          notifications.map((notification) => (
            <div key={notification.followerId} className="notification-item">
              <Avatar photoUrl={notification.followerProfilePicture} />
              <Link
                to={`profile/${notification.followerId}`}
                className="notification-info"
              >
                <p>
                  <span className="notification-username">
                    {notification.followerUsername}
                  </span>{" "}
                  followed you
                </p>
                <p className="notification-time">
                  {new Date(notification.followedAt).toLocaleDateString()}
                </p>
              </Link>
              <button
                title="Remove"
                type="button"
                id="delete-notification-button"
                onClick={() => removeNotification(notification.notificationId)}
              >
                <Close id="close-icon" />
              </button>
            </div>
          ))
        ) : (
          <p className="no-notifications">No notifications</p>
        )}
      </div>
    </div>
  );
};

export default NotificationsDropdown;
