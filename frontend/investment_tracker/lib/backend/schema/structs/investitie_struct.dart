// ignore_for_file: unnecessary_getters_setters

import '/backend/schema/util/schema_util.dart';

import 'index.dart';
import '/flutter_flow/flutter_flow_util.dart';

class InvestitieStruct extends BaseStruct {
  InvestitieStruct({
    String? assetCode,
    String? buyDate,
    double? quantity,
    double? buyPrice,
    double? currentPrice,
    double? pnlUsd,
    double? pnlPct,
    int? id,
  })  : _assetCode = assetCode,
        _buyDate = buyDate,
        _quantity = quantity,
        _buyPrice = buyPrice,
        _currentPrice = currentPrice,
        _pnlUsd = pnlUsd,
        _pnlPct = pnlPct,
        _id = id;

  // "assetCode" field.
  String? _assetCode;
  String get assetCode => _assetCode ?? '';
  set assetCode(String? val) => _assetCode = val;

  bool hasAssetCode() => _assetCode != null;

  // "buyDate" field.
  String? _buyDate;
  String get buyDate => _buyDate ?? '';
  set buyDate(String? val) => _buyDate = val;

  bool hasBuyDate() => _buyDate != null;

  // "quantity" field.
  double? _quantity;
  double get quantity => _quantity ?? 0.0;
  set quantity(double? val) => _quantity = val;

  void incrementQuantity(double amount) => quantity = quantity + amount;

  bool hasQuantity() => _quantity != null;

  // "buyPrice" field.
  double? _buyPrice;
  double get buyPrice => _buyPrice ?? 0.0;
  set buyPrice(double? val) => _buyPrice = val;

  void incrementBuyPrice(double amount) => buyPrice = buyPrice + amount;

  bool hasBuyPrice() => _buyPrice != null;

  // "currentPrice" field.
  double? _currentPrice;
  double get currentPrice => _currentPrice ?? 0.0;
  set currentPrice(double? val) => _currentPrice = val;

  void incrementCurrentPrice(double amount) =>
      currentPrice = currentPrice + amount;

  bool hasCurrentPrice() => _currentPrice != null;

  // "pnlUsd" field.
  double? _pnlUsd;
  double get pnlUsd => _pnlUsd ?? 0.0;
  set pnlUsd(double? val) => _pnlUsd = val;

  void incrementPnlUsd(double amount) => pnlUsd = pnlUsd + amount;

  bool hasPnlUsd() => _pnlUsd != null;

  // "pnlPct" field.
  double? _pnlPct;
  double get pnlPct => _pnlPct ?? 0.0;
  set pnlPct(double? val) => _pnlPct = val;

  void incrementPnlPct(double amount) => pnlPct = pnlPct + amount;

  bool hasPnlPct() => _pnlPct != null;

  // "id" field.
  int? _id;
  int get id => _id ?? 0;
  set id(int? val) => _id = val;

  void incrementId(int amount) => id = id + amount;

  bool hasId() => _id != null;

  static InvestitieStruct fromMap(Map<String, dynamic> data) =>
      InvestitieStruct(
        assetCode: data['assetCode'] as String?,
        buyDate: data['buyDate'] as String?,
        quantity: castToType<double>(data['quantity']),
        buyPrice: castToType<double>(data['buyPrice']),
        currentPrice: castToType<double>(data['currentPrice']),
        pnlUsd: castToType<double>(data['pnlUsd']),
        pnlPct: castToType<double>(data['pnlPct']),
        id: castToType<int>(data['id']),
      );

  static InvestitieStruct? maybeFromMap(dynamic data) => data is Map
      ? InvestitieStruct.fromMap(data.cast<String, dynamic>())
      : null;

  Map<String, dynamic> toMap() => {
        'assetCode': _assetCode,
        'buyDate': _buyDate,
        'quantity': _quantity,
        'buyPrice': _buyPrice,
        'currentPrice': _currentPrice,
        'pnlUsd': _pnlUsd,
        'pnlPct': _pnlPct,
        'id': _id,
      }.withoutNulls;

  @override
  Map<String, dynamic> toSerializableMap() => {
        'assetCode': serializeParam(
          _assetCode,
          ParamType.String,
        ),
        'buyDate': serializeParam(
          _buyDate,
          ParamType.String,
        ),
        'quantity': serializeParam(
          _quantity,
          ParamType.double,
        ),
        'buyPrice': serializeParam(
          _buyPrice,
          ParamType.double,
        ),
        'currentPrice': serializeParam(
          _currentPrice,
          ParamType.double,
        ),
        'pnlUsd': serializeParam(
          _pnlUsd,
          ParamType.double,
        ),
        'pnlPct': serializeParam(
          _pnlPct,
          ParamType.double,
        ),
        'id': serializeParam(
          _id,
          ParamType.int,
        ),
      }.withoutNulls;

  static InvestitieStruct fromSerializableMap(Map<String, dynamic> data) =>
      InvestitieStruct(
        assetCode: deserializeParam(
          data['assetCode'],
          ParamType.String,
          false,
        ),
        buyDate: deserializeParam(
          data['buyDate'],
          ParamType.String,
          false,
        ),
        quantity: deserializeParam(
          data['quantity'],
          ParamType.double,
          false,
        ),
        buyPrice: deserializeParam(
          data['buyPrice'],
          ParamType.double,
          false,
        ),
        currentPrice: deserializeParam(
          data['currentPrice'],
          ParamType.double,
          false,
        ),
        pnlUsd: deserializeParam(
          data['pnlUsd'],
          ParamType.double,
          false,
        ),
        pnlPct: deserializeParam(
          data['pnlPct'],
          ParamType.double,
          false,
        ),
        id: deserializeParam(
          data['id'],
          ParamType.int,
          false,
        ),
      );

  @override
  String toString() => 'InvestitieStruct(${toMap()})';

  @override
  bool operator ==(Object other) {
    return other is InvestitieStruct &&
        assetCode == other.assetCode &&
        buyDate == other.buyDate &&
        quantity == other.quantity &&
        buyPrice == other.buyPrice &&
        currentPrice == other.currentPrice &&
        pnlUsd == other.pnlUsd &&
        pnlPct == other.pnlPct &&
        id == other.id;
  }

  @override
  int get hashCode => const ListEquality().hash([
        assetCode,
        buyDate,
        quantity,
        buyPrice,
        currentPrice,
        pnlUsd,
        pnlPct,
        id
      ]);
}

InvestitieStruct createInvestitieStruct({
  String? assetCode,
  String? buyDate,
  double? quantity,
  double? buyPrice,
  double? currentPrice,
  double? pnlUsd,
  double? pnlPct,
  int? id,
}) =>
    InvestitieStruct(
      assetCode: assetCode,
      buyDate: buyDate,
      quantity: quantity,
      buyPrice: buyPrice,
      currentPrice: currentPrice,
      pnlUsd: pnlUsd,
      pnlPct: pnlPct,
      id: id,
    );
