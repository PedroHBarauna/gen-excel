import logo from "@/assets/logo.svg";

const Header = () => {
  return (
    <header
      className="sticky top-0 z-50 w-full bg-background shadow-header"
      style={{ backgroundColor: "#024ddf" }}
    >
      <div className="container mx-auto flex h-16 items-center px-6">
        <div className="flex items-center gap-2">
          <img
            src={logo}
            alt="Logo"
            className="h-6 w-6 logo"
            style={{ width: "150px", height: "150px" }}
          />
        </div>
      </div>
    </header>
  );
};

export default Header;
