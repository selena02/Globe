import { useEffect, useState } from "react";
import "./Notifications.scss";
import { Notifications } from "@mui/icons-material";
import fetchAPI from "../../utils/fetchAPI";

interface FollowNotification {
  followerId: number;
  followerUsername: string;
  followerProfilePicture: string | null;
  followedAt: Date;
}

export const NotificationsDropdown = () => {
  const [notifications, setNotifications] = useState<FollowNotification[]>([]);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [count, setCount] = useState(0);

  const toggleDropdown = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  useEffect(() => {
    fetchNotifications();
  }, []);

  const fetchNotifications = async () => {
    try {
      const data = await fetchAPI<FollowNotification[]>("users/notifications", {
        method: "GET",
      });
      console.log(data);
      setNotifications(data);
    } catch (error) {
      console.error("Failed to fetch notifications:", error);
    }
  };

  return (
    <div id="notifications-container">
      <button
        title="Notifications-button"
        type="button"
        id="Notifications-button"
      >
        <Notifications id="notifications-icon" />
      </button>
    </div>
  );
};

export default NotificationsDropdown;
