import React, { Component } from "react";
import './EditOrderItem.css';
import { Link } from 'react-router-dom';

export class EditOrderItem extends Component {
    displayName = EditOrderItem.name;
    constructor(props) {
        super(props);
        this.state = {
            value: sessionStorage.getItem('status')
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
                status: this.state.value,
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
            .then(json => {
                this.props.history.push('/oneorder');
            })   
        

       // console.log(receiver)
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
        this.props.history.push('/oneorder');
    }
    render() {
        return (
            <div className="userChange">
                <form>
                    <div className="form-group">
                        <h2>Edycja elementu</h2>
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


                        <label>Status:</label><br />
                        <select onChange={(e) => {
                            this.setState({ value: e.target.value });
                            console.log(this.state)
                        }} >
                            <option value={sessionStorage.getItem('status')}>...</option>
                            <option value="awaiting">Oczekujące</option>
                            <option value="ready">Gotowe</option>
                            <option value="cut">W trakcie</option>
                            <option value="deleted">Usunięte</option>
                        </select>
                    </div>
                  

                    <div className="form-group">
                        <button type="submit" className="cancel_glass_e" onClick={this.cancelItemEdit}>Anuluj</button>
                        <Link to="/oneorder"><button type="submit" className="confirm_glass_e" onClick={this.handleItemEdit}>Zastosuj zmiany</button></Link>
                    </div>
                </form>
            </div>
        );
    }
}