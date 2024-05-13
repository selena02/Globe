import { Cloudinary } from "@cloudinary/url-gen";
import { format, quality } from "@cloudinary/url-gen/actions/delivery";
import { AdvancedImage, placeholder } from "@cloudinary/react";
import { thumbnail } from "@cloudinary/url-gen/actions/resize";
import { autoGravity } from "@cloudinary/url-gen/qualifiers/gravity";
import Profile from "../../features/Profile/Profile";

const cld = new Cloudinary({
  cloud: {
    cloudName: "dsdleukb7",
  },
});

const ProfileImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(200).height(200).gravity(autoGravity()))
    .delivery(format("auto"))
    .delivery(quality("auto"));

  return (
    <AdvancedImage
      cldImg={myImage}
      style={{ maxWidth: "100%" }}
      plugins={[placeholder()]}
    />
  );
};
export default ProfileImg;
