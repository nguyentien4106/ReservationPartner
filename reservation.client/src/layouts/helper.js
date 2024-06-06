import { ACTION_ROUTE_PATH, COLLABORATORS_ROUTE_PATH, COLLABORATOR_ROUTE_PATH, CUSTOMER_ROUTE_PATH, DONATE_ROUTE_PATH } from "../constant/paths";

export const menuItems = (navigate, isAdmin) => isAdmin ? adminItems(navigate) : userItems(navigate)

const userItems = navigate => [
    // {
    //     key: "collaborator",
    //     label: "Cài đặt cho thuê",
    //     children: [
    //         {
    //             key: "collaborator-overall",
    //             label: "Tổng quan",
    //             onClick: () => navigate(COLLABORATOR_ROUTE_PATH.overall),

    //         },
    //         {
    //             key: "collaborator-customer",
    //             label: "Danh sách khách hàng",
    //             onClick: () => navigate(COLLABORATOR_ROUTE_PATH.customer),

    //         },
    //         {
    //             key: "setting",
    //             label: "Chỉnh sửa hồ sơ",
    //             onClick: () => navigate(COLLABORATOR_ROUTE_PATH.setting),
    //         },
    //     ],
    // },

    // {
    //     key: "customer",
    //     label: "Thông tin tài khoản",
    //     children: [
    //         {
    //             key: "customer-overall",
    //             label: "Tổng quan",
    //             onClick: () => navigate(CUSTOMER_ROUTE_PATH.overall),

    //         },
    //         {
    //             key: "order-history",
    //             label: "Lịch sử thuê",
    //             onClick: () => navigate(CUSTOMER_ROUTE_PATH.orderHistory),

    //         },
    //         {
    //             key: "customer-donate",
    //             label: "Lịch sử donate",
    //             onClick: () => navigate(CUSTOMER_ROUTE_PATH.findPeople),
    //         },
    //     ],
    // },
    {
        key: "customer/find-talent",
        label: "Tìm người để thuê",
        onClick: () => navigate(COLLABORATORS_ROUTE_PATH.collaborators)
    },
    {
        key: "post",
        label: "Jobs đang tìm người",
        onClick: () => navigate(CUSTOMER_ROUTE_PATH.jobs)
    },
    {
        key: "donate",
        label: "Donate",
        onClick: () => navigate(DONATE_ROUTE_PATH.donate),
    },
    {
        key: "post",
        label: "Đăng bài",
        children: [
            {
                key: "create-profile",
                label: "Tạo hồ sơ cho thuê",
                onClick: () => navigate(COLLABORATOR_ROUTE_PATH.setting)
            },
            {
                key: "create-job",
                label: "Đăng việc",
                onClick: () => navigate(CUSTOMER_ROUTE_PATH.postJobs)
            },
        ]
    }
];

const adminItems = navigate => [
    {
        key: "manage",
        label: "Quản lý",
        children: [
            {
                key: "manage-collaborator",
                label: "Quản lý Collaborator",
                onClick: () => navigate("/manage-collaborator")
            },
            {
                key: "services",
                label: "Services"
            }
        ]
    }
]

export const getStyles = (screens, token) => ({
    container: {
        alignItems: "center",
        display: "flex",
        justifyContent: "space-between",
        margin: "0 auto",
        padding: screens.md
            ? `0px ${token.paddingLG}px`
            : `0px ${token.padding}px`,
    },
    header: {
        backgroundColor: token.colorBgContainer,
        borderBottom: `${token.lineWidth}px ${token.lineType} ${token.colorSplit}`,
        position: "relative",
    },
    logo: {
        display: "block",
        height: token.sizeLG,
        left: "50%",
        position: screens.md ? "static" : "absolute",
        top: "50%",
        transform: screens.md ? " " : "translate(-50%, -50%)",
    },
    menu: {
        backgroundColor: "transparent",
        borderBottom: "none",
        lineHeight: screens.sm ? "4rem" : "3.5rem",
        marginLeft: screens.md ? "0px" : `-${token.size}px`,
        width: screens.md ? "inherit" : token.sizeXXL,
    },
    menuContainer: {
        alignItems: "center",
        display: "flex",
        gap: token.size,
        width: "100%",
        justifyContent: "space-between",
    },
})