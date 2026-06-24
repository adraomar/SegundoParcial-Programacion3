CREATE TABLE IF NOT EXISTS public.tb_master_control (
    id      INT       GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    fecha   TIMESTAMP NOT NULL DEFAULT NOW(),
    n       INT       NOT NULL,
    coord_x INT       NOT NULL,
    coord_y INT       NOT NULL
);

CREATE TABLE IF NOT EXISTS public.tb_det_log (
    id        INT       GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    master_id INT       NOT NULL REFERENCES tb_master_control(id),
    paso      INT       NOT NULL,
    coord_x   INT       NOT NULL,
    coord_y   INT       NOT NULL
);