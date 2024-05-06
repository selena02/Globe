import { toast } from "react-toastify";

export const handleApiErrors = (error: any) => {
  console.log(error);
  if (error.errors) {
    Object.keys(error.errors).forEach((key) => {
      error.errors
        ? [key].forEach((message) => {
            toast.error(`${key}: ${message}`, {
              position: "bottom-right",
              bodyClassName: "custom-body",
              className: "custom-toast",
              autoClose: 3000,
              hideProgressBar: true,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: true,
              progress: undefined,
            });
          })
        : null;
    });
  } else if (error.message) {
    toast.error(error.message, {
      position: "bottom-right",
      bodyClassName: "custom-body",
      className: "custom-toast",
      autoClose: 3000,
      hideProgressBar: true,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
    });
  } else {
    toast.error("bottom-right", {
      position: "top-right",
      bodyClassName: "custom-body",
      className: "custom-toast",
      autoClose: 3000,
      hideProgressBar: true,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
    });
  }
};
