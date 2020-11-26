import React, { Component } from "react";
import './GlassEdit.css'

export class GlassEdit extends Component {
    displayName = GlassEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    handleGlassEdit = (event) => {
        event.preventDefault();
        var id = [1]
        const receiver = {
            glass: {
                type: this.refs.type.value,
                hight: this.refs.thickness.value,
                width: this.refs.width.value,
                length: this.refs.length.value,
                color: this.refs.color.value,
                owner: this.refs.owner.value,
                desk: sessionStorage.getItem('desk'),
                /*glass_info: { 
                    id: sessionStorage.getItem('id'),
                 }*/
            },
            glass_Ids: {
                id: /*sessionStorage.getItem('id')*/ id
            },
            user: {
                login: sessionStorage.getItem('login')
            }
        }


        fetch(`api/Magazine/Edit_Glass`, {
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
        //this.props.history.push('/oneorder');
        // this.props.history.push('/oneorder');

        console.log(this.refs.color.value)
    }
    cancelGlassEdit = (event) => {
        sessionStorage.removeItem('length');
        sessionStorage.removeItem('width');
        sessionStorage.removeItem('thickness');
        sessionStorage.removeItem('color');
        sessionStorage.removeItem('type');
        sessionStorage.removeItem('amount');
        sessionStorage.removeItem('owner');
        this.props.history.push('/glasswarehouse');
    }
    render() {
        return (
            <div className="userChange">
                <form>
                    <div className="form-group">
                        <h2>Edycja szkła</h2>
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
                        <label>Właściciel</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputOwner"
                            placeholder={sessionStorage.getItem('owner')}
                            defaultValue={sessionStorage.getItem('owner')}
                            ref="owner"
                        />
                    </div>
                    <div className="form-group">
                        <label>Ilość</label>
                        <input
                            type="number"
                            className="form-control"
                            id="inputAmount"
                            min="1"
                            placeholder={sessionStorage.getItem('amount')}
                            defaultValue={sessionStorage.getItem('amount')}
                            ref="amount"
                        />
                    </div>
                    <div className="form-group">
                        <button type="submit" className="cancel_glass_e" onClick={this.cancelGlassEdit}>Anuluj</button>
                        <button type="submit" className="confirm_glass_e" onClick={this.handleGlassEdit}>Zastosuj zmiany</button>

                    </div>

                </form>
            </div>
        );
    }
}