import { Button, Card, Flex, Space } from "antd";
import React from "react";
import "./index.css";
export default function Jobs() {
    return (
        <div style={{
            width: "30%",
            marginLeft: "auto",
            marginRight: "auto",
            height: "100%",
            marginTop: "10%"
        }}>
            <Flex gap={50} vertical justify="center">
                <a href="#" className="card">
                    <Space direction="vertical">
                        <p class="heading gtm-margin-xxs">Việc tìm người</p>
                        <p class="text">
                            Bạn cần tìm một người để phù hợp với yêu cầu của
                            bạn.
                        </p>
                    </Space>
                </a>
                <a href="#" className="card">
                    <Space direction="vertical">
                        <p class="heading gtm-margin-xxs">Người tìm việc</p>
                        <p class="text">
                            Bạn muốn tạo một hồ sơ để người khác có thể tìm thấy
                            bạn và thuê.
                        </p>
                    </Space>
                </a>
            </Flex>
        </div>
    );
}
