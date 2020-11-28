import React, { Component } from "react";
import './EditOrderItem.css'

export class EditOrderItem extends Component {
    displayName = EditOrderItem.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    


    handleItemEdit = (event) => {
        event.preventDefault();
        const receiver = {
            item: {
                type: this.refs.type.value,
                width: this.refs.width.value,
                length: this.refs.length.value,
                color: this.refs.color.value,
                desk: sessionStorage.getItem('desk'),
                status: this.refs.status.value,
                thickness: this.refs.thickness.value,
                id: sessionStorage.getItem('itemId')
            },
            user: {
                login: sessionStorage.getItem('login')
            },
            order:
            {
                id_order: sessionStorage.getItem('orderId')
            }
        }


        fetch(`api/Order/Edit_Order_Items`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
        //this.props.history.push('/one_order');

        console.log(receiver)
    }

    cancelItemEdit = (event) => {
        sessionStorage.removeItem('length');
        sessionStorage.removeItem('width');
        sessionStorage.removeItem('thickness');
        sessionStorage.removeItem('color');
        sessionStorage.removeItem('type');
        sessionStorage.removeItem('status');
        sessionStorage.removeItem('desk');
        sessionStorage.removeItem('itemId');
        this.props.history.push('/one_order');
    }
    render() {
        return (
            <div className="userChange">
                <form>
                    <div className="form-group">
                        <h2>Edycja element</h2>
                        <label>Długość</label>
                        <input
                            type="number"
                            className="form-control"
                            id="inputLength"
                            placeholder={sessionStorage.getItem('length')}
                            defaultValue={sessionStorage.getItem('length')}
                            ref="length"
                        />
                    </div>
                    <div className="form-group">
                        <label>Szerokość</label>
                        <input
                            type="number"
                            className="form-control"
                            id="inputWidth"
                            placeholder={sessionStorage.getItem('width')}
                            defaultValue={sessionStorage.getItem('width')}
                            ref="width"
                        />
                    </div>
                    <div className="form-group">
                        <label>Typ</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputWidth"
                            placeholder={sessionStorage.getItem('type')}
                            defaultValue={sessionStorage.getItem('type')}
                            ref="type"
                        />
                    </div>
                    <div className="form-group">
                        <label>Grubość</label>
                        <input
                            type="number"
                            className="form-control"
                            id="inputThickness"
                            placeholder={sessionStorage.getItem('thickness')}
                            defaultValue={sessionStorage.getItem('thickness')}
                            ref="thickness"
                        />
                    </div>
                    <div className="form-group">
                        <label>Kolor</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputColor"
                            placeholder={sessionStorage.getItem('color')}
                            defaultValue={sessionStorage.getItem('color')}
                            ref="color"
                        />
                    </div>
                    <div className="form-group">
                        <label>Status</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputStatus"
                            placeholder={sessionStorage.getItem('status')}
                            defaultValue={sessionStorage.getItem('status')}
                            ref="status"
                        />
                    </div>
                  

                    <div className="form-group">
                        <button type="submit" className="cancel_glass_e" onClick={this.cancelItemEdit}>Anuluj</button>
                        <button type="submit" className="confirm_glass_e" onClick={this.handleItemEdit}>Zastosuj zmiany</button>
                    </div>
                </form>
            </div>
        );
    }
}