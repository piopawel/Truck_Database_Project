CREATE OR REPLACE PROCEDURE deleteOrder(p_id ORDERS.ORDER_ID%TYPE)
AS
v_order orders%ROWTYPE;
BEGIN
SELECT * into v_order from orders where ORDER_ID=p_id;
IF (v_order.start_date>SYSDATE) THEN
  DELETE FROM orders where order_id=p_id;
END IF;
END;